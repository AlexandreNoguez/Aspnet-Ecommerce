using AspnetEcommerce.Domain.Category.Entity;
using AspnetEcommerce.Domain.Product.Repository;
using AspnetEcommerce.Infrastructure.Database;
using AspnetEcommerce.Infrastructure.Product.Mappers;
using AspnetEcommerce.Infrastructure.Product.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.Infrastructure.Product.Repository;

public sealed class CategoryRepositoryEf : ICategoryRepository
{
    private readonly DatabaseContext _db;

    public CategoryRepositoryEf(DatabaseContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task AddAsync(CategoryEntity category, CancellationToken ct = default)
    {
        var model = CategoryMapper.ToDbModel(category);
        await _db.Categories.AddAsync(model, ct);
    }

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default)
    {
        var normalizedSlug = slug.Trim().ToLowerInvariant();

        return await _db.Categories
            .AnyAsync(c => c.Slug == normalizedSlug, ct);
    }

    public async Task<CategoryPagedResult> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        IQueryable<CategoryDbModel> query = _db.Categories;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{normalizedSearch}%"));
        }

        var totalItems = await query.CountAsync(ct);

        var categories = await query
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = categories
            .Select(CategoryMapper.ToDomain)
            .ToList();

        return new CategoryPagedResult(items, totalItems);
    }

    public async Task<CategoryEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return model is null ? null : CategoryMapper.ToDomain(model);
    }

    public async Task<IReadOnlyList<CategoryEntity>> GetAllAsync(CancellationToken ct = default)
    {
        var models = await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(ct);

        return models.Select(CategoryMapper.ToDomain).ToList();
    }

    public async Task UpdateAsync(CategoryEntity category, CancellationToken ct = default)
    {
        var model = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id, ct);
        if (model is null) throw new InvalidOperationException("Category not found.");

        CategoryMapper.ApplyToDbModel(category, model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (model is null) return;

        _db.Categories.Remove(model);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (model is null) return;

        model.IsActive = false;
    }
}
