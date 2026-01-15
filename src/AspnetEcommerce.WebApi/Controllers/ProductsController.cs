using AspnetEcommerce.Application.Product.DTOs.CreateProduct;
using AspnetEcommerce.Application.Product.UseCases.CreateProduct;
using AspnetEcommerce.WebApi.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly ICreateProductUseCase _createProductUseCase;

    public ProductsController(ICreateProductUseCase createProductUseCase)
    {
        _createProductUseCase = createProductUseCase ?? throw new ArgumentNullException(nameof(createProductUseCase));
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var input = new CreateProductInput(
            request.Name,
            request.Description,
            request.Amount,
            request.Currency,
            request.Sku,
            request.CategoryId,
            request.StockQuantity);

        var result = await _createProductUseCase.ExecuteAsync(input, cancellationToken);

        var response = new CreateProductResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Amount = result.Amount,
            Currency = result.Currency,
            Sku = result.Sku,
            CategoryId = result.CategoryId,
            StockQuantity = result.StockQuantity,
            IsActive = result.IsActive
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(new { id });
    }
}
