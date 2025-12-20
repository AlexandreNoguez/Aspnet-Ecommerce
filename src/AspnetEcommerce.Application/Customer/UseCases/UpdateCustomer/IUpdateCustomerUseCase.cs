using AspnetEcommerce.Application.Customer.DTOs.UpdateCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.UpdateCustomer;

public interface IUpdateCustomerUseCase
{
    Task<UpdateCustomerOutput> ExecuteAsync(UpdateCustomerInput input, CancellationToken cancellationToken = default);
}