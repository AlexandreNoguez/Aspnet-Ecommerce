using AspnetEcommerce.Application.Product.DTOs.DeleteProduct;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Product.UseCases.DeleteProduct;

public sealed class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProductUseCase> _logger;

    public DeleteProductUseCase(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteProductUseCase> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync(DeleteProductInput input, CancellationToken cancellationToken = default)
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
            await _productRepository.DeleteAsync(input.Id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while deleting product {ProductId}", input.Id);
            throw;
        }
    }
}
