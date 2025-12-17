using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;
using AspnetEcommerce.WebApi.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICreateCustomerUseCase _createCustomerUseCase;

    public CustomersController(ICreateCustomerUseCase createCustomerUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCustomerResponse>> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        // Mapeia Request -> Command (Application)
        var command = new CreateCustomerInput(
            request.Name,
            request.Email,
            request.Street,
            request.City,
            request.State,
            request.ZipCode,
            request.Number
        );

        var result = await _createCustomerUseCase.ExecuteAsync(command, cancellationToken);

        var response = new CreateCustomerResponse
        {
            Id = result.Id,
            Name = result.Name,
            IsActive = result.IsActive,
            RewardPoints = result.RewardPoints
        };

        // 201 Created com Location
        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }

    // Só para poder usar no CreatedAtAction (pode implementar depois)
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<object>> GetById(Guid id, CancellationToken ct)
    {
        // Aqui depois você injeta e usa o IGetCustomerUseCase

        return Ok(new { Id = id });
    }
}
