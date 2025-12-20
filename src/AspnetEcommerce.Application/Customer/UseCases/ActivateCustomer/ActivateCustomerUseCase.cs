using AspnetEcommerce.Application.Customer.DTOs.ActivateCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Activation;
using AspnetEcommerce.Domain.Customer.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Customer.UseCases.ActivateCustomer;

public sealed class ActivateCustomerUseCase : IActivateCustomerUseCase
{
    private readonly ICustomerActivationTokenRepository _activationTokenRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateCustomerUseCase> _logger;

    public ActivateCustomerUseCase(
        ICustomerActivationTokenRepository activationTokenRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ILogger<ActivateCustomerUseCase> logger)
    {
        _activationTokenRepository = activationTokenRepository ?? throw new ArgumentNullException(nameof(activationTokenRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ActivateCustomerOutput> ExecuteAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ValidationException("Activation token is required.");

        // 1) Busca token
        var activation = await _activationTokenRepository
            .GetByTokenAsync(token, cancellationToken);

        if (activation is null)
            throw new ValidationException("Activation link is invalid or has already been used.");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // 2) Valida se token está válido
            activation.EnsureIsValid();

            // 3) Busca Customer
            var customer = await _customerRepository
                .GetByIdAsync(activation.CustomerId, cancellationToken);

            if (customer is null)
                throw new ValidationException("Customer not found for provided activation token.");

            // 4) Ativa o cliente (regra de domínio)
            customer.Activate();

            // 5) Marca token como usado
            activation.MarkAsUsed();

            await _customerRepository.UpdateAsync(customer, cancellationToken);
            await _activationTokenRepository.UpdateAsync(activation, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ActivateCustomerOutput(
                customer.Id,
                customer.Email,
                customer.IsActive);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while activating customer using token {Token}", token);
            throw;
        }
    }
}
