using AspnetEcommerce.Application.Product.DTOs.UpdateProduct;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.ValueObject;
using AspnetEcommerce.Domain.Product.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Product.UseCases.UpdateProduct;

public sealed class UpdateProductUseCase : IUpdateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProductUseCase> _logger;

    public UpdateProductUseCase(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateProductUseCase> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UpdateProductOutput> ExecuteAsync(
        UpdateProductInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty) throw new ValidationException("Id is required.");
        if (string.IsNullOrWhiteSpace(input.Name)) throw new ValidationException("Name is required.");
        if (string.IsNullOrWhiteSpace(input.Description)) throw new ValidationException("Description is required.");
        if (string.IsNullOrWhiteSpace(input.Sku)) throw new ValidationException("Sku is required.");
        if (string.IsNullOrWhiteSpace(input.Currency)) throw new ValidationException("Currency is required.");
        if (input.CategoryId == Guid.Empty) throw new ValidationException("CategoryId is required.");
        if (input.Amount <= 0) throw new ValidationException("Amount must be greater than zero.");
        if (input.StockQuantity < 0) throw new ValidationException("StockQuantity cannot be negative.");

        var product = await _productRepository.GetByIdAsync(input.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException($"Product with id '{input.Id}' was not found.");
        }

        var normalizedSku = input.Sku.Trim();
        if (!string.Equals(product.Sku.Value, normalizedSku, StringComparison.OrdinalIgnoreCase))
        {
            var skuExists = await _productRepository.SkuExistsAsync(normalizedSku, cancellationToken);
            if (skuExists)
            {
                throw new ValidationException("Sku is already in use.");
            }
        }

        var category = await _categoryRepository.GetByIdAsync(input.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{input.CategoryId}' was not found.");
        }

        if (!category.IsActive)
        {
            throw new ValidationException("Category must be active.");
        }

        var name = ProductName.Create(input.Name);
        var description = ProductDescription.Create(input.Description);
        var price = Money.Create(input.Amount, input.Currency);
        var sku = Sku.Create(normalizedSku);

        product.ChangeName(name);
        product.ChangeDescription(description);
        product.ChangePrice(price);
        product.ChangeSku(sku);
        product.ChangeCategory(category);
        product.SetStockQuantity(input.StockQuantity);

        if (input.IsActive)
        {
            product.Activate();
        }
        else
        {
            product.Deactivate();
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while updating product {ProductId}", input.Id);
            throw;
        }

        return new UpdateProductOutput(
            product.Id,
            product.Name.Value,
            product.Description.Value,
            product.Price.Amount,
            product.Price.Currency,
            product.Sku.Value,
            product.Category.Id,
            product.Category.Name,
            product.StockQuantity,
            product.IsActive);
    }
}
