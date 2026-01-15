namespace AspnetEcommerce.Application.Product.DTOs.CreateCategory;

public sealed record CreateCategoryInput(
    string Name,
    string Slug,
    string? Description
);
