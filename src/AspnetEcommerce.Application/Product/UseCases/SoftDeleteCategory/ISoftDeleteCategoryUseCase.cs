using AspnetEcommerce.Application.Product.DTOs.SoftDeleteCategory;

namespace AspnetEcommerce.Application.Product.UseCases.SoftDeleteCategory;

public interface ISoftDeleteCategoryUseCase
{
    Task ExecuteAsync(SoftDeleteCategoryInput input, CancellationToken cancellationToken = default);
}
