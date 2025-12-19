namespace AspnetEcommerce.Application.Customer.DTOs.ActivateCustomer
{
    public sealed record ActivateCustomerOutput(
        Guid CustomerId,
        string Email,
        bool IsActive);
}
