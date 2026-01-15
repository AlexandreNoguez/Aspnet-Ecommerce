namespace AspnetEcommerce.Application.Product.DTOs.UpdateProduct;

public sealed record UpdateProductInput(
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
