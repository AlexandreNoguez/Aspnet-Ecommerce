namespace AspnetEcommerce.Application.Product.DTOs.GetAllCategories;

public sealed record GetAllCategoriesItemOutput(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    int DisplayOrder
);

public sealed record GetAllCategoriesOutput(
    List<GetAllCategoriesItemOutput> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);
