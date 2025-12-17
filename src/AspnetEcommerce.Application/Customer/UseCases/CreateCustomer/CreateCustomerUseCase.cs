using AspnetEcommerce.Application.Contracts.Email.Jobs;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Factory;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Domain.Customer.ValueObject;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;

public sealed class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailJobQueue _emailJobQueue;
    private readonly ILogger<CreateCustomerUseCase> _logger;

    public CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IEmailJobQueue emailJobQueue,
        ILogger<CreateCustomerUseCase> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _emailJobQueue = emailJobQueue ?? throw new ArgumentNullException(nameof(emailJobQueue));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private static string BuildActivationLink(Guid customerId)
    {
        const string baseUrl = "https://localhost:5021/api/customers/activate";
        // TODO: futuramente trocar customerId por token de ativação
        return $"{baseUrl}?customerId={customerId}";
    }

    public async Task<CreateCustomerOutput> ExecuteAsync(
        CreateCustomerInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        // 1) Validations at Application layer
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new ValidationException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Email))
        {
            throw new ValidationException("Email is required.");
        }

        var emailAlreadyExists = await _customerRepository
            .EmailExistsAsync(input.Email, cancellationToken);

        if (emailAlreadyExists)
        {
            throw new ValidationException("Email is already in use.");
        }

        // Vamos guardar só os dados primitivos aqui,
        // pra não depender do tipo Customer fora do try
        Guid customerId;
        string customerName;
        string customerEmail;
        bool isActive;
        int rewardPoints;

        // 2) Database operations (with rollback ONLY for DB errors)
        try
        {
            var address = Address.Create(
                input.Street,
                input.City,
                input.State,
                input.ZipCode,
                input.Number
            );

            var customer = CustomerFactory.CreateNewCustomer(input.Name, input.Email, address);

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            // Copia os dados que vamos precisar depois
            customerId = customer.Id;
            customerName = customer.Name;
            customerEmail = customer.Email;
            isActive = customer.IsActive;
            rewardPoints = customer.RewardPoints;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw; // DB error still fails the request
        }

        // 3) Enqueue welcome email job (DO NOT break the flow if it fails)
        try
        {
            var activationLink = BuildActivationLink(customerId);

            var job = new WelcomeEmailJob(
                customerId: customerId,
                customerName: customerName,
                customerEmail: customerEmail,
                activationLink: activationLink);

            await _emailJobQueue.EnqueueWelcomeEmailAsync(job, cancellationToken);
        }
        catch (Exception ex)
        {
            // Just log – do NOT rethrow
            _logger.LogError(
                ex,
                "Failed to enqueue welcome email for customer {CustomerId} ({Email})",
                customerId,
                customerEmail);
        }

        // 4) Return success (customer was persisted)
        return new CreateCustomerOutput(
            customerId,
            customerName,
            customerEmail,
            isActive,
            rewardPoints
        );
    }
}
