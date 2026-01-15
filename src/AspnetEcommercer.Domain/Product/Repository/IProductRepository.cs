using AspnetEcommerce.Domain.Product.Entity;

namespace AspnetEcommerce.Domain.Product.Repository;

public interface IProductRepository
{
    Task AddAsync(ProductEntity product, CancellationToken ct = default);
    Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default);
    Task<ProductPagedResult> GetPagedAsync(int page, int pageSize, string? search, Guid? categoryId, CancellationToken ct = default);
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductEntity>> GetAllAsync(CancellationToken ct = default);
    Task UpdateAsync(ProductEntity product, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed record ProductPagedResult(
    IReadOnlyList<ProductEntity> Items,
    int TotalItems
);
