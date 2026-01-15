namespace AspnetEcommerce.Application.Product.DTOs.GetAllProducts;

public sealed record GetAllProductsInput(
    int Page,
    int PageSize,
    string? Search,
    Guid? CategoryId
);



