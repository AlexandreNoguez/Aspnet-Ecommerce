namespace AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;

public sealed record CreateCustomerOutput(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    int RewardPoints
);
