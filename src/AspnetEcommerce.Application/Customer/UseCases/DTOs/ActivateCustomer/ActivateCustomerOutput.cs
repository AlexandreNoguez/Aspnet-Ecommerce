namespace AspnetEcommerce.Application.Customer.UseCases.ActivateCustomer
{
    public sealed record ActivateCustomerOutput(
        Guid CustomerId,
        string Email,
        bool IsActive);
}
