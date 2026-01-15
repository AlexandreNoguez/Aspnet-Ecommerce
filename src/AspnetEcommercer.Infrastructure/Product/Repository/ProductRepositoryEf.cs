using AspnetEcommerce.Domain.Product.Entity;
using AspnetEcommerce.Domain.Product.Repository;
using AspnetEcommerce.Infrastructure.Database;
using AspnetEcommerce.Infrastructure.Product.Mappers;
using AspnetEcommerce.Infrastructure.Product.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.Infrastructure.Product.Repository;

public sealed class ProductRepositoryEf : IProductRepository
{
    private readonly DatabaseContext _db;

    public ProductRepositoryEf(DatabaseContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task AddAsync(ProductEntity product, CancellationToken ct = default)
    {
        var model = ProductMapper.ToDbModel(product);
        await _db.Products.AddAsync(model, ct);
    }

    public async Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default)
    {
        var normalizedSku = sku.Trim();

        return await _db.Products
            .AnyAsync(p => p.Sku == normalizedSku, ct);
    }

    public async Task<ProductPagedResult> GetPagedAsync(int page, int pageSize, string? search, Guid? categoryId, CancellationToken ct = default)
    {
        IQueryable<ProductDbModel> query = _db.Products;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, $"%{normalizedSearch}%") ||
                EF.Functions.ILike(p.Description, $"%{normalizedSearch}%") ||
                EF.Functions.ILike(p.Sku, $"%{normalizedSearch}%"));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalItems = await query.CountAsync(ct);

        var products = await query
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Sku)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = products
            .Select(p => p.Category is null
                ? null
                : ProductMapper.ToDomain(p, p.Category))
            .Where(p => p is not null)
            .Cast<ProductEntity>()
            .ToList();

        return new ProductPagedResult(items, totalItems);
    }

    public async Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        if (model is null || model.Category is null)
        {
            return null;
        }

        return ProductMapper.ToDomain(model, model.Category);
    }

    public async Task<IReadOnlyList<ProductEntity>> GetAllAsync(CancellationToken ct = default)
    {
        var models = await _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Sku)
            .ToListAsync(ct);

        return models
            .Where(p => p.Category is not null)
            .Select(p => ProductMapper.ToDomain(p, p.Category!))
            .ToList();
    }

    public async Task UpdateAsync(ProductEntity product, CancellationToken ct = default)
    {
        var model = await _db.Products.FirstOrDefaultAsync(p => p.Id == product.Id, ct);
        if (model is null) throw new InvalidOperationException("Product not found.");

        ProductMapper.ApplyToDbModel(product, model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (model is null) return;

        _db.Products.Remove(model);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (model is null) return;

        model.IsActive = false;
    }
}
