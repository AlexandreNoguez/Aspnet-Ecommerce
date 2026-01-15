namespace AspnetEcommerce.Application.Product.DTOs.GetProductById;

public sealed record GetProductByIdOutput(
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
