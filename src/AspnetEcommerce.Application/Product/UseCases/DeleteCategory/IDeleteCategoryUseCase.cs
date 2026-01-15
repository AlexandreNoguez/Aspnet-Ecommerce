using AspnetEcommerce.Application.Product.DTOs.DeleteCategory;

namespace AspnetEcommerce.Application.Product.UseCases.DeleteCategory;

public interface IDeleteCategoryUseCase
{
    Task ExecuteAsync(DeleteCategoryInput input, CancellationToken cancellationToken = default);
}
