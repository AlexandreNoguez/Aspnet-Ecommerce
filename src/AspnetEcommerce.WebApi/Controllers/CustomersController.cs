using AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;
using AspnetEcommerce.Application.Customer.DTOs.GetAllCustomer;
using AspnetEcommerce.Application.Customer.DTOs.GetCustomerById;
using AspnetEcommerce.Application.Customer.UseCases.ActivateCustomer;
using AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;
using AspnetEcommerce.Application.Customer.UseCases.GetAllCustomers;
using AspnetEcommerce.Application.Customer.UseCases.GetCustomerById;
using AspnetEcommerce.WebApi.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspnetEcommerce.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICreateCustomerUseCase _createCustomerUseCase;
    private readonly IActivateCustomerUseCase _activateCustomerUseCase;
    private readonly IGetCustomerByIdUseCase _getCustomerByIdUseCase;
    private readonly IGetAllCustomersUseCase _getAllCustomersUseCase;
    public CustomersController(
         ICreateCustomerUseCase createCustomerUseCase,
         IActivateCustomerUseCase activateCustomerUseCase,
        IGetCustomerByIdUseCase getCustomerByIdUseCase,
        IGetAllCustomersUseCase getAllCustomersUseCase)

    {
        _createCustomerUseCase = createCustomerUseCase ?? throw new ArgumentNullException(nameof(createCustomerUseCase));
        _activateCustomerUseCase = activateCustomerUseCase ?? throw new ArgumentNullException(nameof(activateCustomerUseCase));
        _getCustomerByIdUseCase = getCustomerByIdUseCase ?? throw new ArgumentNullException(nameof(getCustomerByIdUseCase));
        _getAllCustomersUseCase = getAllCustomersUseCase ?? throw new ArgumentNullException(nameof(getAllCustomersUseCase));
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

    [HttpGet]
    public async Task<ActionResult<GetAllCustomersOutput>> GetAll(
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10,
      [FromQuery] string? search = null,
      CancellationToken cancellationToken = default)
    {
        var input = new GetAllCustomersInput(page, pageSize, search);
        var output = await _getAllCustomersUseCase.ExecuteAsync(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet("activate")]
    public async Task<IActionResult> Activate(
    [FromQuery] string token,
    CancellationToken cancellationToken)
    {
        try
        {

            var output = await _activateCustomerUseCase.ExecuteAsync(token, cancellationToken);

            // Aqui você pode:
            // - retornar JSON
            // - ou redirecionar para uma página de frontend
            return Ok(new
            {
                message = "Account activated successfully.",
                customerId = output.CustomerId,
                email = output.Email,
                isActive = output.IsActive
            });
        }
        catch (Exception ex)
        {
            // Trate erros adequadamente
            return BadRequest(new { error = ex.Message });
        }
    }

    // Só para poder usar no CreatedAtAction (pode implementar depois)
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetCustomerByIdOutput>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var input = new GetCustomerByIdInput(id);
        var output = await _getCustomerByIdUseCase.ExecuteAsync(input, cancellationToken);
        return Ok(output);
    }
}
