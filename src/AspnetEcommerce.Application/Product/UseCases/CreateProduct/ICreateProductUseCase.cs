using AspnetEcommerce.Application.Product.DTOs.CreateProduct;

namespace AspnetEcommerce.Application.Product.UseCases.CreateProduct
{
    public interface ICreateProductUseCase
    {
        Task<CreateProductOutput> ExecuteAsync(CreateProductInput input, CancellationToken ct = default);
    }
}
