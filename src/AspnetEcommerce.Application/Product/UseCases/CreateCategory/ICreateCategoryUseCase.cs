using AspnetEcommerce.Application.Product.DTOs.CreateCategory;

namespace AspnetEcommerce.Application.Product.UseCases.CreateCategory
{
    public interface ICreateCategoryUseCase
    {
        Task<CreateCategoryOutput> ExecuteAsync(CreateCategoryInput input, CancellationToken ct = default);
    }
}
