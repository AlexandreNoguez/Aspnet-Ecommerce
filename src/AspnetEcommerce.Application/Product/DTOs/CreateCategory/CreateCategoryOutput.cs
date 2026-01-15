namespace AspnetEcommerce.Application.Product.DTOs.CreateCategory;

public sealed record CreateCategoryOutput(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    int DisplayOrder
);
