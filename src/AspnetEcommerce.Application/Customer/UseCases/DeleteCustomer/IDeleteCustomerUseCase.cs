using AspnetEcommerce.Application.Customer.DTOs.DeleteCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.DeleteCustomer;

public interface IDeleteCustomerUseCase
{
    Task ExecuteAsync(DeleteCustomerInput input, CancellationToken cancellationToken = default);
};