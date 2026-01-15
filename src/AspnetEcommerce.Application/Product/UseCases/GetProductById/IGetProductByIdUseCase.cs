using AspnetEcommerce.Application.Product.DTOs.GetProductById;

namespace AspnetEcommerce.Application.Product.UseCases.GetProductById;

public interface IGetProductByIdUseCase
{
    Task<GetProductByIdOutput> ExecuteAsync(GetProductByIdInput input, CancellationToken ct = default);
}
