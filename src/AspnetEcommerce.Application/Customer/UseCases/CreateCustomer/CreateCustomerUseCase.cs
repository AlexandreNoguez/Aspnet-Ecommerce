using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommercer.Domain.Contracts.Abstractions;
using AspnetEcommercer.Domain.Customer.Factory;
using AspnetEcommercer.Domain.Customer.Repository;
using AspnetEcommercer.Domain.Customer.ValueObject;

namespace AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;

public sealed class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateCustomerOutput> ExecuteAsync(
        CreateCustomerInput input,
        CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var address = Address.Create(
                input.Street,
                input.City,
                input.State,
                input.ZipCode,
                input.Number
            );

            var customer = CustomerFactory.CreateNewCustomer(input.Name, address);

            await _customerRepository.AddAsync(customer, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreateCustomerOutput(
                customer.Id,
                customer.Name,
                customer.IsActive,
                customer.RewardPoints
            );
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
