using AspnetEcommerce.Application.Product.DTOs.SoftDeleteCategory;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Product.UseCases.SoftDeleteCategory;

public sealed class SoftDeleteCategoryUseCase : ISoftDeleteCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeleteCategoryUseCase> _logger;

    public SoftDeleteCategoryUseCase(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeleteCategoryUseCase> logger)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync(SoftDeleteCategoryInput input, CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty) throw new ValidationException("Id is required.");

        var existingCategory = await _categoryRepository.GetByIdAsync(input.Id, cancellationToken);
        if (existingCategory is null)
        {
            throw new NotFoundException($"Category with id '{input.Id}' was not found.");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _categoryRepository.SoftDeleteAsync(input.Id, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while soft deleting category {CategoryId}", input.Id);
            throw;
        }
    }
}
