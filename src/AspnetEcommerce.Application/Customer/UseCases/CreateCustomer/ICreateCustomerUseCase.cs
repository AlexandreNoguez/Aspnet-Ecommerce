using AspnetEcommerce.Application.Customer.UseCases.DTOs.CreateCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.CreateCustomer
{
    public interface ICreateCustomerUseCase
    {
        Task<CreateCustomerOutput> ExecuteAsync(CreateCustomerInput input, CancellationToken cancellationToken);
    }
}
