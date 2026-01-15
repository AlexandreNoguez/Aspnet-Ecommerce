using AspnetEcommerce.Application.Product.DTOs.GetAllProducts;

namespace AspnetEcommerce.Application.Product.UseCases.GetAllProducts;

public interface IGetAllProductsUseCase
{
    Task<GetAllProductsOutput> ExecuteAsync(
        GetAllProductsInput input,
        CancellationToken cancellationToken = default);
}
