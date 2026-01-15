using AspnetEcommerce.Application.Product.DTOs.CreateProduct;
using AspnetEcommerce.Application.Product.DTOs.DeleteProduct;
using AspnetEcommerce.Application.Product.DTOs.GetAllProducts;
using AspnetEcommerce.Application.Product.DTOs.GetProductById;
using AspnetEcommerce.Application.Product.DTOs.SoftDeleteProduct;
using AspnetEcommerce.Application.Product.DTOs.UpdateProduct;
using AspnetEcommerce.Application.Product.UseCases.CreateProduct;
using AspnetEcommerce.Application.Product.UseCases.DeleteProduct;
using AspnetEcommerce.Application.Product.UseCases.GetAllProducts;
using AspnetEcommerce.Application.Product.UseCases.GetProductById;
using AspnetEcommerce.Application.Product.UseCases.SoftDeleteProduct;
using AspnetEcommerce.Application.Product.UseCases.UpdateProduct;
using AspnetEcommerce.WebApi.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly ICreateProductUseCase _createProductUseCase;
    private readonly IGetAllProductsUseCase _getAllProductsUseCase;
    private readonly IGetProductByIdUseCase _getProductByIdUseCase;
    private readonly IUpdateProductUseCase _updateProductUseCase;
    private readonly IDeleteProductUseCase _deleteProductUseCase;
    private readonly ISoftDeleteProductUseCase _softDeleteProductUseCase;

    public ProductsController(
        ICreateProductUseCase createProductUseCase,
        IGetAllProductsUseCase getAllProductsUseCase,
        IGetProductByIdUseCase getProductByIdUseCase,
        IUpdateProductUseCase updateProductUseCase,
        IDeleteProductUseCase deleteProductUseCase,
        ISoftDeleteProductUseCase softDeleteProductUseCase)
    {
        _createProductUseCase = createProductUseCase ?? throw new ArgumentNullException(nameof(createProductUseCase));
        _getAllProductsUseCase = getAllProductsUseCase ?? throw new ArgumentNullException(nameof(getAllProductsUseCase));
        _getProductByIdUseCase = getProductByIdUseCase ?? throw new ArgumentNullException(nameof(getProductByIdUseCase));
        _updateProductUseCase = updateProductUseCase ?? throw new ArgumentNullException(nameof(updateProductUseCase));
        _deleteProductUseCase = deleteProductUseCase ?? throw new ArgumentNullException(nameof(deleteProductUseCase));
        _softDeleteProductUseCase = softDeleteProductUseCase ?? throw new ArgumentNullException(nameof(softDeleteProductUseCase));
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

    [HttpGet]
    public async Task<ActionResult<GetAllProductsOutput>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] Guid? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var input = new GetAllProductsInput(page, pageSize, search, categoryId);
        var output = await _getAllProductsUseCase.ExecuteAsync(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetProductByIdOutput>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var input = new GetProductByIdInput(id);
        var output = await _getProductByIdUseCase.ExecuteAsync(input, cancellationToken);

        return Ok(output);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateProductInput(
            id,
            request.Name,
            request.Description,
            request.Amount,
            request.Currency,
            request.Sku,
            request.CategoryId,
            request.StockQuantity,
            request.IsActive);

        var result = await _updateProductUseCase.ExecuteAsync(input, cancellationToken);

        var response = new UpdateProductResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Amount = result.Amount,
            Currency = result.Currency,
            Sku = result.Sku,
            CategoryId = result.CategoryId,
            CategoryName = result.CategoryName,
            StockQuantity = result.StockQuantity,
            IsActive = result.IsActive
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteProductInput(id);
        await _deleteProductUseCase.ExecuteAsync(input, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}/soft")]
    public async Task<IActionResult> SoftDeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var input = new SoftDeleteProductInput(id);
        await _softDeleteProductUseCase.ExecuteAsync(input, cancellationToken);

        return NoContent();
    }
}