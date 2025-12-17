namespace AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;

public sealed record CreateCustomerInput(
    string Name,
    string Email,
    string Street,
    string City,
    string State,
    string ZipCode,
    int Number
);
