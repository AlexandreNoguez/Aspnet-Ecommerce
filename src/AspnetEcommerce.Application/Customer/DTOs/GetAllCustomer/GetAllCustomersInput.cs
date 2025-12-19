namespace AspnetEcommerce.Application.Customer.DTOs.GetAllCustomer;

public sealed record GetAllCustomersInput(
    int Page,
    int PageSize,
    string? Search
);
