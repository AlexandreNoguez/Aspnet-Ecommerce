using AspnetEcommerce.Application.Customer.DTOs.GetCustomerById;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Customer.Repository;

namespace AspnetEcommerce.Application.Customer.UseCases.GetCustomerByIdUseCase;

public sealed class GetCustomerByIdUseCase : IGetCustomerByIdUseCase
{
    private readonly ICustomerRepository _repo;

    public GetCustomerByIdUseCase(ICustomerRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<GetCustomerByIdOutput> ExecuteAsync(GetCustomerByIdInput input, CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty)
            throw new ValidationException("Id is required.");

        var customer = await _repo.GetByIdAsync(input.Id, ct);
        if (customer is null)
            throw new NotFoundException($"Customer with id '{input.Id}' was not found.");

        return new GetCustomerByIdOutput(
            customer.Id,
            customer.Name,
            customer.Email,
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