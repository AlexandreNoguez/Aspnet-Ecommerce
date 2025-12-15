namespace AspnetEcommercer.WebApi.Models.Customers;

public sealed class CreateCustomerResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public bool IsActive { get; init; }
    public int RewardPoints { get; init; }
}
