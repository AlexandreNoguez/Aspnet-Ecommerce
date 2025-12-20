using AspnetEcommerce.Domain.Customer.Entity;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Infrastructure.Customer.Mappers;
using AspnetEcommerce.Infrastructure.Customer.Models;
using AspnetEcommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.Infrastructure.Customer.Repository;

public class CustomerRepositoryEf : ICustomerRepository
{
    private readonly DatabaseContext _db;

    public CustomerRepositoryEf(DatabaseContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await _db.Customers
            .AnyAsync(c => c.Email == normalizedEmail, cancellationToken);
    }

    public async Task AddAsync(CustomerEntity customer, CancellationToken ct = default)
    {
        var model = CustomerMapper.ToDbModel(customer);
        await _db.Customers.AddAsync(model, ct);

        // IMPORTANT: no SaveChanges here (use UnitOfWork)
        // await _db.SaveChangesAsync(ct);
    }

    public async Task<CustomerPagedResult> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        IQueryable<CustomerDbModel> query = _db.Customers;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(c =>
                EF.Functions.ILike(c.Name, $"%{normalizedSearch}%") ||
                EF.Functions.ILike(c.Email, $"%{normalizedSearch}%"));
        }

        var totalItems = await query.CountAsync(ct);

        var customers = await query
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = customers
            .Select(CustomerMapper.ToDomain)
            .ToList();

        return new CustomerPagedResult(items, totalItems);
    }

    public async Task UpdateAsync(CustomerEntity customer, CancellationToken ct = default)
    {
        var model = await _db.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id, ct);
        if (model is null) throw new InvalidOperationException("Customer not found.");

        CustomerMapper.ApplyToDbModel(customer, model);
        // no SaveChanges here
    }

    public async Task<CustomerEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return model is null ? null : CustomerMapper.ToDomain(model);
    }

    public async Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken ct = default)
    {
        var models = await _db.Customers
            .AsNoTracking()
            .ToListAsync(ct);

        return models.Select(CustomerMapper.ToDomain).ToList() ?? [];
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (model is null) return;

        _db.Customers.Remove(model);
        // no SaveChanges here
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (model is null) return;
        model.IsActive = false;
    }
}
