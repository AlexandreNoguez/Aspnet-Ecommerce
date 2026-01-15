using AspnetEcommerce.Domain.Category.Entity;

namespace AspnetEcommerce.Domain.Product.Repository;

public interface ICategoryRepository
{
    Task AddAsync(CategoryEntity category, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);
    Task<CategoryPagedResult> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<CategoryEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CategoryEntity>> GetAllAsync(CancellationToken ct = default);
    Task UpdateAsync(CategoryEntity category, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
}

public sealed record CategoryPagedResult(
    IReadOnlyList<CategoryEntity> Items,
    int TotalItems
);
