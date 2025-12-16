namespace AspnetEcommerce.Infrastructure.Customer.Models
{
    public class CustomerDbModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        // Flattened address columns (OK)
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public int? Number { get; set; }

        public bool IsActive { get; set; }
        public int RewardPoints { get; set; }
    }
}
