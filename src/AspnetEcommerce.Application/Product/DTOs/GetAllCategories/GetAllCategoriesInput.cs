namespace AspnetEcommerce.Application.Product.DTOs.GetAllCategories;

public sealed record GetAllCategoriesInput(
    int Page,
    int PageSize,
    string? Search
);

