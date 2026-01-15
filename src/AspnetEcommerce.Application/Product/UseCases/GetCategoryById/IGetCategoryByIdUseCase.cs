using AspnetEcommerce.Application.Product.DTOs.GetProductById;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.GetProductById;

public sealed class GetProductByIdUseCase : IGetProductByIdUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<GetProductByIdOutput> ExecuteAsync(GetProductByIdInput input, CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty)
        {
            throw new ValidationException("Id is required.");
        }

        var product = await _productRepository.GetByIdAsync(input.Id, ct);
        if (product is null)
        {
            throw new NotFoundException($"Product with id '{input.Id}' was not found.");
        }

        return new GetProductByIdOutput(
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
