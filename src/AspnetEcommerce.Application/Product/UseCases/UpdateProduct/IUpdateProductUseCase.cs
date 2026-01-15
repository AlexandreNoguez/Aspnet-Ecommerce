using AspnetEcommerce.Application.Product.DTOs.UpdateProduct;

namespace AspnetEcommerce.Application.Product.UseCases.UpdateProduct;

public interface IUpdateProductUseCase
{
    Task<UpdateProductOutput> ExecuteAsync(UpdateProductInput input, CancellationToken cancellationToken = default);
}