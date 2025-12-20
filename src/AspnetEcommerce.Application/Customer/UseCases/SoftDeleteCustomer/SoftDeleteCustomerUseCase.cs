
using AspnetEcommerce.Application.Customer.DTOs.SoftDeleteCustomer;
using AspnetEcommerce.Application.Customer.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Customer.UseCases.SoftDeleteCustomer
{
    public class SoftDeleteCustomerUseCase : ISoftDeleteCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SoftDeleteCustomerUseCase> _logger;

        public SoftDeleteCustomerUseCase(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            ILogger<SoftDeleteCustomerUseCase> logger)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync(SoftDeleteCustomerInput input, CancellationToken cancellationToken = default)
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
                await _customerRepository.SoftDeleteAsync(input.Id, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error while soft deleting customer {CustomerId}", input.Id);
                throw;
            }
        }
    }
}
