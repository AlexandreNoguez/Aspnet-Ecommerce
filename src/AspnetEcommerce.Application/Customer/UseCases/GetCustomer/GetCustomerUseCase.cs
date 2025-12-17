using AspnetEcommerce.Application.Customer.UseCases.DTOs.GetCustomer;
using AspnetEcommerce.Domain.Customer.Repository;

namespace AspnetEcommerce.Application.Customer.UseCases.GetCustomer;

public sealed class GetCustomerUseCase
{
    private readonly ICustomerRepository _repo;

    public GetCustomerUseCase(ICustomerRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<GetCustomerOutput?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        var customer = await _repo.GetByIdAsync(id, ct);
        if (customer is null) return null;

        return new GetCustomerOutput(
            customer.Id,
            customer.Name,
            customer.IsActive,
            customer.RewardPoints,
            customer.Address?.Street,
            customer.Address?.City,
            customer.Address?.State,
            customer.Address?.ZipCode,
            customer.Address?.Number
        );
    }
}
