using AspnetEcommerce.Application.Product.DTOs.UpdateCategory;

namespace AspnetEcommerce.Application.Product.UseCases.UpdateCategory;

public interface IUpdateCategoryUseCase
{
    Task<UpdateCategoryOutput> ExecuteAsync(UpdateCategoryInput input, CancellationToken cancellationToken = default);
}
