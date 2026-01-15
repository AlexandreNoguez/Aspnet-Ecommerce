namespace AspnetEcommerce.Application.Product.DTOs.GetCategoryById;

public sealed record GetCategoryByIdOutput(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    int DisplayOrder
);
