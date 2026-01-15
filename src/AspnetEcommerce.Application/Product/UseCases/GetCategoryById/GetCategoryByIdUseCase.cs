using AspnetEcommerce.Application.Product.DTOs.GetCategoryById;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.GetCategoryById;

public sealed class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<GetCategoryByIdOutput> ExecuteAsync(GetCategoryByIdInput input, CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty)
        {
            throw new ValidationException("Id is required.");
        }

        var category = await _categoryRepository.GetByIdAsync(input.Id, ct);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{input.Id}' was not found.");
        }

        return new GetCategoryByIdOutput(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IsActive,
            category.DisplayOrder);
    }
}
