using AspnetEcommerce.Application.Product.DTOs.CreateProduct;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Factory;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.CreateProduct;

public sealed class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductUseCase(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateProductOutput> ExecuteAsync(CreateProductInput input, CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new ValidationException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Description))
        {
            throw new ValidationException("Description is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Sku))
        {
            throw new ValidationException("Sku is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Currency))
        {
            throw new ValidationException("Currency is required.");
        }

        if (input.CategoryId == Guid.Empty)
        {
            throw new ValidationException("CategoryId is required.");
        }

        if (input.Amount <= 0)
        {
            throw new ValidationException("Amount must be greater than zero.");
        }

        if (input.StockQuantity < 0)
        {
            throw new ValidationException("StockQuantity cannot be negative.");
        }

        var skuExists = await _productRepository.SkuExistsAsync(input.Sku, ct);
        if (skuExists)
        {
            throw new ValidationException("Sku is already in use.");
        }

        var category = await _categoryRepository.GetByIdAsync(input.CategoryId, ct);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{input.CategoryId}' was not found.");
        }

        if (!category.IsActive)
        {
            throw new ValidationException("Category must be active.");
        }

        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var product = ProductFactory.CreateNewProduct(
                input.Name,
                input.Description,
                input.Amount,
                input.Currency,
                input.Sku,
                category,
                input.StockQuantity);

            await _productRepository.AddAsync(product, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreateProductOutput(
                product.Id,
                product.Name.Value,
                product.Description.Value,
                product.Price.Amount,
                product.Price.Currency,
                product.Sku.Value,
                product.Category.Id,
                product.StockQuantity,
                product.IsActive
            );
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
