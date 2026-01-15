using AspnetEcommerce.Application.Product.DTOs.UpdateCategory;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Repository;
using Microsoft.Extensions.Logging;

namespace AspnetEcommerce.Application.Product.UseCases.UpdateCategory;

public sealed class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCategoryUseCase> _logger;

    public UpdateCategoryUseCase(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateCategoryUseCase> logger)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UpdateCategoryOutput> ExecuteAsync(
        UpdateCategoryInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        if (input.Id == Guid.Empty) throw new ValidationException("Id is required.");
        if (string.IsNullOrWhiteSpace(input.Name)) throw new ValidationException("Name is required.");
        if (string.IsNullOrWhiteSpace(input.Slug)) throw new ValidationException("Slug is required.");
        if (input.DisplayOrder < 0) throw new ValidationException("DisplayOrder cannot be negative.");

        var category = await _categoryRepository.GetByIdAsync(input.Id, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{input.Id}' was not found.");
        }

        var normalizedSlug = input.Slug.Trim().ToLowerInvariant();
        if (!string.Equals(category.Slug, normalizedSlug, StringComparison.Ordinal))
        {
            var slugExists = await _categoryRepository.SlugExistsAsync(normalizedSlug, cancellationToken);
            if (slugExists)
            {
                throw new ValidationException("Slug is already in use.");
            }
        }

        category.SetName(input.Name);
        category.SetSlug(normalizedSlug);
        category.SetDescription(input.Description);
        category.SetDisplayOrder(input.DisplayOrder);

        if (input.IsActive)
        {
            category.Activate();
        }
        else
        {
            category.Deactivate();
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _categoryRepository.UpdateAsync(category, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error while updating category {CategoryId}", input.Id);
            throw;
        }

        return new UpdateCategoryOutput(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IsActive,
            category.DisplayOrder);
    }
}
