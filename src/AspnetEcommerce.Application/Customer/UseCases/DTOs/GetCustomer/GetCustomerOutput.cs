namespace AspnetEcommerce.Application.Customer.UseCases.DTOs.GetCustomer;

public sealed record GetCustomerOutput(
    Guid Id,
    string Name,
    bool IsActive,
    int RewardPoints,
    string? Street,
    string? City,
    string? State,
    string? ZipCode,
    int? Number
);
