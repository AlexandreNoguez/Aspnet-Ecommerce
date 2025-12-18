using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspnetEcommerce.Domain.Customer.Activation
{
    public interface ICustomerActivationTokenRepository
    {
        Task AddAsync(CustomerActivationToken token, CancellationToken ct = default);
        Task<CustomerActivationToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    }
}
