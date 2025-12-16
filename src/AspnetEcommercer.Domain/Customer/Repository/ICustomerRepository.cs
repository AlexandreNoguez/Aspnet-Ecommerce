using AspnetEcommerce.Domain.Customer.Entity;

namespace AspnetEcommerce.Domain.Customer.Repository;

public interface ICustomerRepository
{
    Task AddAsync(CustomerEntity customer, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    Task UpdateAsync(CustomerEntity customer, CancellationToken ct = default);
    Task<CustomerEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
