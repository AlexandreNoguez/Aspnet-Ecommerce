using AspnetEcommercer.Domain.Customer.Entity;
using AspnetEcommercer.Domain.Customer.Repository;
using AspnetEcommercer.Infrastructure.Customer.Mappers;
using AspnetEcommercer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommercer.Infrastructure.Customer.Repository;

public class CustomerRepositoryEf : ICustomerRepository
{
    private readonly DatabaseContext _db;

    public CustomerRepositoryEf(DatabaseContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task AddAsync(CustomerEntity customer, CancellationToken ct = default)
    {
        var model = CustomerMapper.ToDbModel(customer);
        await _db.Customers.AddAsync(model, ct);

        // IMPORTANT: no SaveChanges here (use UnitOfWork)
        // await _db.SaveChangesAsync(ct);
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

        return models.Select(CustomerMapper.ToDomain).ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (model is null) return;

        _db.Customers.Remove(model);
        // no SaveChanges here
    }
}
