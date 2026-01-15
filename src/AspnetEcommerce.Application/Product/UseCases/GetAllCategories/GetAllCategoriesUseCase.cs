using AspnetEcommerce.Application.Product.DTOs.GetAllCategories;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.GetAllCategories;

public sealed class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<GetAllCategoriesOutput> ExecuteAsync(
        GetAllCategoriesInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        var (page, pageSize) = NormalizePagination(input.Page, input.PageSize);

        var result = await _categoryRepository.GetPagedAsync(page, pageSize, input.Search, cancellationToken);

        var totalPages = CalculateTotalPages(result.TotalItems, pageSize);

        var items = result.Items
            .Select(category => new GetAllCategoriesItemOutput(
                category.Id,
                category.Name,
                category.Slug,
                category.Description,
                category.IsActive,
                category.DisplayOrder))
            .ToList();

        return new GetAllCategoriesOutput(
            items,
            page,
            pageSize,
            result.TotalItems,
            totalPages);
    }

    private static (int Page, int PageSize) NormalizePagination(int page, int pageSize)
    {
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        return (normalizedPage, normalizedPageSize);
    }

    private static int CalculateTotalPages(int totalItems, int pageSize)
    {
        if (totalItems == 0) return 0;

        return (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
