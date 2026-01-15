using AspnetEcommerce.Application.Product.DTOs.CreateCategory;
using AspnetEcommerce.Application.Product.Exceptions;
using AspnetEcommerce.Domain.Category.Factory;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.CreateCategory;

public sealed class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<CreateCategoryOutput> ExecuteAsync(CreateCategoryInput input, CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new ValidationException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Slug))
        {
            throw new ValidationException("Slug is required.");
        }

        var slugExists = await _categoryRepository.SlugExistsAsync(input.Slug, ct);
        if (slugExists)
        {
            throw new ValidationException("Slug is already in use.");
        }

        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var category = CategoryFactory.CreateNewCategory(input.Name, input.Slug, input.Description);

            await _categoryRepository.AddAsync(category, ct);
            await _unitOfWork.CommitAsync(ct);

            return new CreateCategoryOutput(
                category.Id,
                category.Name,
                category.Slug,
                category.Description,
                category.IsActive,
                category.DisplayOrder
            );
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
