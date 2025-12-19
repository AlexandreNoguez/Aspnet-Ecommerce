
using AspnetEcommerce.Application.Customer.DTOs.GetAllCustomer;

namespace AspnetEcommerce.Application.Customer.UseCases.GetAllCustomers;

public interface IGetAllCustomersUseCase
{
    Task<GetAllCustomersOutput> ExecuteAsync(
        GetAllCustomersInput input,
        CancellationToken cancellationToken = default);
}