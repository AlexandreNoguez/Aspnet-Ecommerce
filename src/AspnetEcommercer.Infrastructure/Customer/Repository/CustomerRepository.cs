using AspnetEcommercer.Infrastructure.Customer.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommercer.Infrastructure.Customer.Repository
{
    public class CustomerRepository
    {
        private readonly DbContext _dbContext;

        public CustomerRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateCustomer(CustomerDbModel customer)
        {
            _dbContext.Set<CustomerDbModel>().Add(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCustomer(CustomerDbModel customer)
        {
            _dbContext.Set<CustomerDbModel>().Update(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CustomerDbModel?> GetCustomerById(Guid id)
        {
            return await _dbContext.Set<CustomerDbModel>().FindAsync(id);
        }

        public async Task<List<CustomerDbModel>> GetAllCustomers()
        {
            return await _dbContext.Set<CustomerDbModel>().ToListAsync();
        }

        public async Task DeleteCustomer(Guid id)
        {
            var customer = await _dbContext.Set<CustomerDbModel>().FindAsync(id);
            if (customer != null)
            {
                _dbContext.Set<CustomerDbModel>().Remove(customer);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
