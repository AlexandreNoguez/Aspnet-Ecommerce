using AspnetEcommerce.Application.Product.DTOs.GetAllCategories;

namespace AspnetEcommerce.Application.Product.UseCases.GetAllCategories;

public interface IGetAllCategoriesUseCase
{
    Task<GetAllCategoriesOutput> ExecuteAsync(
        GetAllCategoriesInput input,
        CancellationToken cancellationToken = default);
}
