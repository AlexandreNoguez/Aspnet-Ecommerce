namespace AspnetEcommerce.Application.Customer.UseCases.DTOs.CreateCustomer;

public sealed record CreateCustomerInput(
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode,
    int Number
);
