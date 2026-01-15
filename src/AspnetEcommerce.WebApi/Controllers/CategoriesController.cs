using AspnetEcommerce.Application.Product.DTOs.CreateCategory;
using AspnetEcommerce.Application.Product.DTOs.DeleteCategory;
using AspnetEcommerce.Application.Product.DTOs.GetAllCategories;
using AspnetEcommerce.Application.Product.DTOs.GetCategoryById;
using AspnetEcommerce.Application.Product.DTOs.SoftDeleteCategory;
using AspnetEcommerce.Application.Product.DTOs.UpdateCategory;
using AspnetEcommerce.Application.Product.UseCases.CreateCategory;
using AspnetEcommerce.Application.Product.UseCases.DeleteCategory;
using AspnetEcommerce.Application.Product.UseCases.GetAllCategories;
using AspnetEcommerce.Application.Product.UseCases.GetCategoryById;
using AspnetEcommerce.Application.Product.UseCases.SoftDeleteCategory;
using AspnetEcommerce.Application.Product.UseCases.UpdateCategory;
using AspnetEcommerce.WebApi.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICreateCategoryUseCase _createCategoryUseCase;
    private readonly IGetAllCategoriesUseCase _getAllCategoriesUseCase;
    private readonly IGetCategoryByIdUseCase _getCategoryByIdUseCase;
    private readonly IUpdateCategoryUseCase _updateCategoryUseCase;
    private readonly IDeleteCategoryUseCase _deleteCategoryUseCase;
    private readonly ISoftDeleteCategoryUseCase _softDeleteCategoryUseCase;

    public CategoriesController(
        ICreateCategoryUseCase createCategoryUseCase,
        IGetAllCategoriesUseCase getAllCategoriesUseCase,
        IGetCategoryByIdUseCase getCategoryByIdUseCase,
        IUpdateCategoryUseCase updateCategoryUseCase,
        IDeleteCategoryUseCase deleteCategoryUseCase,
        ISoftDeleteCategoryUseCase softDeleteCategoryUseCase)
    {
        _createCategoryUseCase = createCategoryUseCase ?? throw new ArgumentNullException(nameof(createCategoryUseCase));
        _getAllCategoriesUseCase = getAllCategoriesUseCase ?? throw new ArgumentNullException(nameof(getAllCategoriesUseCase));
        _getCategoryByIdUseCase = getCategoryByIdUseCase ?? throw new ArgumentNullException(nameof(getCategoryByIdUseCase));
        _updateCategoryUseCase = updateCategoryUseCase ?? throw new ArgumentNullException(nameof(updateCategoryUseCase));
        _deleteCategoryUseCase = deleteCategoryUseCase ?? throw new ArgumentNullException(nameof(deleteCategoryUseCase));
        _softDeleteCategoryUseCase = softDeleteCategoryUseCase ?? throw new ArgumentNullException(nameof(softDeleteCategoryUseCase));
    }

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCategoryInput(
            request.Name,
            request.Slug,
            request.Description);

        var result = await _createCategoryUseCase.ExecuteAsync(input, cancellationToken);

        var response = new CreateCategoryResponse
        {
            Id = result.Id,
            Name = result.Name,
            Slug = result.Slug,
            Description = result.Description,
            IsActive = result.IsActive,
            DisplayOrder = result.DisplayOrder
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllCategoriesOutput>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var input = new GetAllCategoriesInput(page, pageSize, search);
        var output = await _getAllCategoriesUseCase.ExecuteAsync(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetCategoryByIdOutput>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var input = new GetCategoryByIdInput(id);
        var output = await _getCategoryByIdUseCase.ExecuteAsync(input, cancellationToken);

        return Ok(output);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory(
        Guid id,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCategoryInput(
            id,
            request.Name,
            request.Slug,
            request.Description,
            request.IsActive,
            request.DisplayOrder);

        var result = await _updateCategoryUseCase.ExecuteAsync(input, cancellationToken);

        var response = new UpdateCategoryResponse
        {
            Id = result.Id,
            Name = result.Name,
            Slug = result.Slug,
            Description = result.Description,
            IsActive = result.IsActive,
            DisplayOrder = result.DisplayOrder
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteCategoryInput(id);
        await _deleteCategoryUseCase.ExecuteAsync(input, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<IActionResult> SoftDeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var input = new SoftDeleteCategoryInput(id);
        await _softDeleteCategoryUseCase.ExecuteAsync(input, cancellationToken);

        return NoContent();
    }
}