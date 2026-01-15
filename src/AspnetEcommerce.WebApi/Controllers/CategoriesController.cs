using AspnetEcommerce.Application.Product.DTOs.CreateCategory;
using AspnetEcommerce.Application.Product.UseCases.CreateCategory;
using AspnetEcommerce.WebApi.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICreateCategoryUseCase _createCategoryUseCase;

    public CategoriesController(ICreateCategoryUseCase createCategoryUseCase)
    {
        _createCategoryUseCase = createCategoryUseCase ?? throw new ArgumentNullException(nameof(createCategoryUseCase));
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

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(new { id });
    }
}
