namespace AspnetEcommerce.Application.Customer.DTOs.CreateCustomer;

public sealed record CreateCustomerOutput(
    Guid Id,
    string Name,
    bool IsActive,
    int RewardPoints
);
