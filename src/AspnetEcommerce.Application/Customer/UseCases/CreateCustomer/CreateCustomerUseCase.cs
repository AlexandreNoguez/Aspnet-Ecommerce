using AspnetEcommerce.Application.Contracts.Email.Jobs;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using AspnetEcommerce.Application.Customer.Contracts.Links;
using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Activation;
using AspnetEcommerce.Domain.Customer.Entity;
using AspnetEcommerce.Domain.Customer.Factory;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Domain.Customer.ValueObject;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;

public sealed class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerActivationTokenRepository _activationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailJobQueue _emailJobQueue;
    private readonly ILogger<CreateCustomerUseCase> _logger;
    private readonly IActivationLinkBuilder _activationLinkBuilder;

    public CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        ICustomerActivationTokenRepository activationTokenRepository,
        IUnitOfWork unitOfWork,
        IEmailJobQueue emailJobQueue,
        ILogger<CreateCustomerUseCase> logger,
        IActivationLinkBuilder activationLinkBuilder)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _activationTokenRepository = activationTokenRepository ?? throw new ArgumentNullException(nameof(activationTokenRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _emailJobQueue = emailJobQueue ?? throw new ArgumentNullException(nameof(emailJobQueue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _activationLinkBuilder = activationLinkBuilder ?? throw new ArgumentNullException(nameof(activationLinkBuilder));
    }

    private static string GenerateSecureToken()
    {
        // 32 bytes => 256 bits
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes); // vai ser usado em querystring com EscapeDataString
    }

    public async Task<CreateCustomerOutput> ExecuteAsync(
        CreateCustomerInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        // 1) Validações básicas
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ValidationException("Name is required.");

        if (string.IsNullOrWhiteSpace(input.Email))
            throw new ValidationException("Email is required.");

        var emailAlreadyExists = await _customerRepository
            .EmailExistsAsync(input.Email, cancellationToken);

        if (emailAlreadyExists)
            throw new ValidationException("Email is already in use.");

        CustomerEntity customer;
        CustomerActivationToken activationToken;

        // 2) Persiste Customer + ActivationToken (mesmo UnitOfWork)
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var address = Address.Create(
                input.Street,
                input.City,
                input.State,
                input.ZipCode,
                input.Number
            );

            customer = CustomerFactory.CreateNewCustomer(input.Name, input.Email, address);

            await _customerRepository.AddAsync(customer, cancellationToken);

            var tokenValue = GenerateSecureToken();
            var expiresAt = DateTimeOffset.UtcNow.AddHours(24);

            activationToken = CustomerActivationToken.CreateNew(
                customerId: customer.Id,
                token: tokenValue,
                expiresAt: expiresAt);

            await _activationTokenRepository.AddAsync(activationToken, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while creating customer {Email}", input.Email);
            throw;
        }

        // 3) Enfileira e-mail de boas-vindas + link de ativação (não quebra o fluxo se falhar)
        try
        {
            var activationLink = _activationLinkBuilder.Build(activationToken.Token);

            var job = new WelcomeEmailJob(
                customerId: customer.Id,
                customerName: customer.Name,
                customerEmail: customer.Email,
                activationLink: activationLink);

            await _emailJobQueue.EnqueueWelcomeEmailAsync(job, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to enqueue welcome email for customer {CustomerId} ({Email})",
                customer.Id,
                customer.Email);
            // Não lança exceção – criação do cliente já foi persistida.
        }

        // 4) Retorno
        return new CreateCustomerOutput(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.IsActive,
            customer.RewardPoints
        );
    }
}
