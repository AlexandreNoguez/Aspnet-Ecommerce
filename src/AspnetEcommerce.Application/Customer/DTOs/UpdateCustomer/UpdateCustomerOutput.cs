namespace AspnetEcommerce.Application.Customer.DTOs.UpdateCustomer;

public sealed record UpdateCustomerOutput(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    int RewardPoints,
    string Street,
    string City,
    string State,
    string ZipCode,
    int Number
);