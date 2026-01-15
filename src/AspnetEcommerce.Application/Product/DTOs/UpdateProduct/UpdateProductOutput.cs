namespace AspnetEcommerce.Application.Product.DTOs.UpdateProduct;

public sealed record UpdateProductOutput(
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
