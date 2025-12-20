using AspnetEcommerce.Domain.Customer.Activation;
using AspnetEcommerce.Infrastructure.Customer.Mappers;
using AspnetEcommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.Infrastructure.Customer.Repository
{
    public class CustomerActivationTokenRepositoryEf : ICustomerActivationTokenRepository
    {
        private readonly DatabaseContext _context;

        public CustomerActivationTokenRepositoryEf(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerActivationToken token, CancellationToken ct = default)
        {
            var model = CustomerActivationTokenMapper.ToDbModel(token);

            await _context.CustomerActivationTokens.AddAsync(model, ct);
        }

        public async Task<CustomerActivationToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        {
            var model = await _context.CustomerActivationTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Token == token, ct);

            if (model is null)
                return null;

            return CustomerActivationTokenMapper.ToDomain(model);
        }

        public async Task UpdateAsync(CustomerActivationToken token, CancellationToken ct = default)
        {
            var model = await _context.CustomerActivationTokens
                .FirstOrDefaultAsync(x => x.Id == token.Id, ct);

            if (model is null)
                throw new InvalidOperationException("Activation token not found.");

            model.UsedAt = token.UsedAt;
        }
    }
}
