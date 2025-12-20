using AspnetEcommerce.Application.Customer.DTOs.UpdateCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Domain.Customer.ValueObject;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Customer.UseCases.UpdateCustomer;

public sealed class UpdateCustomerUseCase : IUpdateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCustomerUseCase> _logger;

    public UpdateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCustomerUseCase> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UpdateCustomerOutput> ExecuteAsync(
        UpdateCustomerInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty) throw new ValidationException("Id is required.");
        if (string.IsNullOrWhiteSpace(input.Name)) throw new ValidationException("Name is required.");
        if (string.IsNullOrWhiteSpace(input.Email)) throw new ValidationException("Email is required.");

        var customer = await _customerRepository.GetByIdAsync(input.Id, cancellationToken);
        if (customer is null)
            throw new NotFoundException($"Customer with id '{input.Id}' was not found.");

        var normalizedEmail = input.Email.Trim();
        if (!string.Equals(customer.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase))
        {
            var emailAlreadyExists = await _customerRepository.EmailExistsAsync(normalizedEmail, cancellationToken);
            if (emailAlreadyExists)
                throw new ValidationException("Email is already in use.");
        }

        var address = Address.Create(
            input.Street,
            input.City,
            input.State,
            input.ZipCode,
            input.Number);

        customer.SetName(input.Name);
        customer.SetEmail(normalizedEmail);
        customer.ChangeAddress(address);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _customerRepository.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while updating customer {CustomerId}", input.Id);
            throw;
        }

        return new UpdateCustomerOutput(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.IsActive,
            customer.RewardPoints,
            customer.Address!.Street,
            customer.Address.City,
            customer.Address.State,
            customer.Address.ZipCode,
            customer.Address.Number);
    }
}