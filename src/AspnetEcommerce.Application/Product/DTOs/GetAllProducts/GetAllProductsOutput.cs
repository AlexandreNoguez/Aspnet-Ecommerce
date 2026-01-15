namespace AspnetEcommerce.Application.Product.DTOs.GetAllProducts;

public sealed record GetAllProductsItemOutput(
    Guid Id,
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    string Sku,
    Guid CategoryId,
    string CategoryName,
    int StockQuantity,
    bool IsActive
);

public sealed record GetAllProductsOutput(
    List<GetAllProductsItemOutput> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);
