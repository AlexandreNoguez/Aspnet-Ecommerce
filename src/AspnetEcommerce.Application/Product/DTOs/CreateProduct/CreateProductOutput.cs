namespace AspnetEcommerce.Application.Product.DTOs.CreateProduct;

public sealed record CreateProductOutput(
    Guid Id,
    string Name,
    string Description,
    decimal Amount,
    string Currency,
    string Sku,
    Guid CategoryId,
    int StockQuantity,
    bool IsActive
);
