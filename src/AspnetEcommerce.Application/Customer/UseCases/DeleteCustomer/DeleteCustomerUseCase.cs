using AspnetEcommerce.Application.Customer.DTOs.DeleteCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Customer.UseCases.DeleteCustomer;

public sealed class DeleteCustomerUseCase : IDeleteCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCustomerUseCase> _logger;

    public DeleteCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteCustomerUseCase> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync(DeleteCustomerInput input, CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty)
            throw new ValidationException("Id is required.");

        var existingCustomer = await _customerRepository.GetByIdAsync(input.Id, cancellationToken);
        if (existingCustomer is null)
            throw new NotFoundException($"Customer with id '{input.Id}' was not found.");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _customerRepository.DeleteAsync(input.Id, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while deleting customer {CustomerId}", input.Id);
            throw;
        }
    }
}