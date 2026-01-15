using AspnetEcommerce.Application.Product.DTOs.SoftDeleteProduct;

namespace AspnetEcommerce.Application.Product.UseCases.SoftDeleteProduct;

public interface ISoftDeleteProductUseCase
{
    Task ExecuteAsync(SoftDeleteProductInput input, CancellationToken cancellationToken = default);
}
