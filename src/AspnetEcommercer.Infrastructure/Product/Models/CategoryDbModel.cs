namespace AspnetEcommerce.Infrastructure.Product.Models
{
    public class CategoryDbModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
