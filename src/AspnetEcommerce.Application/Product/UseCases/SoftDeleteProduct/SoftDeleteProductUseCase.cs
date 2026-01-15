using AspnetEcommerce.Application.Product.DTOs.SoftDeleteProduct;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Product.UseCases.SoftDeleteProduct;

public sealed class SoftDeleteProductUseCase : ISoftDeleteProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteProductUseCase> _logger;

    public SoftDeleteProductUseCase(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeleteProductUseCase> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync(SoftDeleteProductInput input, CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty) throw new ValidationException("Id is required.");

        var existingProduct = await _productRepository.GetByIdAsync(input.Id, cancellationToken);
        if (existingProduct is null)
        {
            throw new NotFoundException($"Product with id '{input.Id}' was not found.");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _productRepository.SoftDeleteAsync(input.Id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while soft deleting product {ProductId}", input.Id);
            throw;
        }
    }
}
