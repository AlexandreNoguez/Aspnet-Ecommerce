using AspnetEcommerce.Application.Product.DTOs.DeleteProduct;

namespace AspnetEcommerce.Application.Product.UseCases.DeleteProduct;

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(DeleteProductInput input, CancellationToken cancellationToken = default);
}
