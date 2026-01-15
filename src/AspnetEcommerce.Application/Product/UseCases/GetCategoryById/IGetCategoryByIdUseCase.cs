using AspnetEcommerce.Application.Product.DTOs.GetCategoryById;

namespace AspnetEcommerce.Application.Product.UseCases.GetCategoryById;

public interface IGetCategoryByIdUseCase
{
    Task<GetCategoryByIdOutput> ExecuteAsync(GetCategoryByIdInput input, CancellationToken ct = default);
}
