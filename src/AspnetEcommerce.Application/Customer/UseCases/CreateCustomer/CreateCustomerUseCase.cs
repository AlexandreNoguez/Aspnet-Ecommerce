using AspnetEcommerce.Application.Customer.UseCases.DTOs.CreateCustomer;
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
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        // Basic application-level validation (cheap checks)
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new ArgumentException("Name is required.", nameof(input.Name));
        }
        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
