using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;

public interface ICreateCustomerUseCase
{
    Task<CreateCustomerOutput> ExecuteAsync(CreateCustomerInput input, CancellationToken ct = default);
}
