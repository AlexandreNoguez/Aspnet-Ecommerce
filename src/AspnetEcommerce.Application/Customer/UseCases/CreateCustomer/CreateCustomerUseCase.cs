using AspnetEcommercer.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommercer.Domain.Contracts.Abstractions;
using AspnetEcommercer.Domain.Customer.Entity;
using AspnetEcommercer.Domain.Customer.Repository;
using AspnetEcommercer.Domain.Customer.ValueObject;

public sealed class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(ICustomerRepository repo, IUnitOfWork uow)
    {
        _customerRepository = repo ?? throw new ArgumentNullException(nameof(repo));
        _unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
    }

    public async Task<CreateCustomerOutput> ExecuteAsync(CreateCustomerInput input, CancellationToken ct = default)
    {
        // Basic application-level validation (cheap checks)
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Name is required.", nameof(input.Name));

        // Build domain objects
        var address = new Address(input.Street, input.City, input.State, input.ZipCode, input.Number);
        address.Validate(); // or rely on constructor rules if you make it immutable later

        var customer = new CustomerEntity(
            Guid.NewGuid(),
            input.Name,
            address,
            isActive: false,
            rewardPoints: 0
        );

        await _customerRepository.AddAsync(customer, ct);
        await _unitOfWork.CommitAsync();

        return new CreateCustomerOutput(customer.Id);
    }
}
