using AspnetEcommercer.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommercer.Domain.Contracts.Abstractions;
using AspnetEcommercer.Domain.Customer.Entity;
using AspnetEcommercer.Domain.Customer.Repository;
using AspnetEcommercer.Domain.Customer.ValueObject;

public sealed class CreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateCustomerOutput> ExecuteAsync(CreateCustomerInput input, CancellationToken ct = default)
    {
        // Basic application-level validation (cheap checks)
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Name is required.", nameof(input.Name));

        // Build domain objects
        var address = Address.Create(input.Street, input.City, input.State, input.ZipCode, input.Number);

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
