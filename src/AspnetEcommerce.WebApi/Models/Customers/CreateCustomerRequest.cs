namespace AspnetEcommerce.WebApi.Models.Customers;

public sealed class CreateCustomerRequest
{
    public string Name { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public int Number { get; set; }
}
