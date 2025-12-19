using AspnetEcommerce.Application.Customer.DTOs.GetCustomerById;

namespace AspnetEcommerce.Application.Customer.UseCases.GetCustomerById
{
    public interface IGetCustomerByIdUseCase
    {
        Task<GetCustomerByIdOutput> ExecuteAsync(GetCustomerByIdInput input, CancellationToken ct = default);
    }
}
