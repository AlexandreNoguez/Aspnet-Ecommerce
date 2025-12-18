using System.Threading;
using System.Threading.Tasks;

namespace AspnetEcommerce.Application.Customer.UseCases.ActivateCustomer
{
    public interface IActivateCustomerUseCase
    {
        Task<ActivateCustomerOutput> ExecuteAsync(
            string token,
            CancellationToken cancellationToken = default);
    }
}
