
using AspnetEcommerce.Application.Contracts.Email.Jobs;

namespace AspnetEcommerce.Application.Customer.Contracts.Email
{
    public interface IEmailJobQueue
    {
        Task EnqueueWelcomeEmailAsync(WelcomeEmailJob job, CancellationToken ct = default);
    }
}
