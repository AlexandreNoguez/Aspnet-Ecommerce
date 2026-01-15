namespace AspnetEcommerce.Application.Product.DTOs.UpdateCategory;

public sealed record UpdateCategoryOutput(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    int DisplayOrder
);
