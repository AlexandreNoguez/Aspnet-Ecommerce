namespace AspnetEcommerce.Application.Customer.DTOs.UpdateCustomer;

public sealed record UpdateCustomerInput(
    Guid Id,
    string Name,
    string Email,
    string Street,
    string City,
    string State,
    string ZipCode,
    int Number,
    int RewardPoints
);