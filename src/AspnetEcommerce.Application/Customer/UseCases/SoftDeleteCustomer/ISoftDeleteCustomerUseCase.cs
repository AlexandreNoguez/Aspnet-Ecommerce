using AspnetEcommerce.Application.Customer.DTOs.SoftDeleteCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.SoftDeleteCustomer;

public interface ISoftDeleteCustomerUseCase
{
    Task ExecuteAsync(SoftDeleteCustomerInput input, CancellationToken cancellationToken = default);
};