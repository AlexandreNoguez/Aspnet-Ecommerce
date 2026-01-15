namespace AspnetEcommerce.Infrastructure.Product.Models
{
    public class ProductDbModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Sku { get; set; }
        public decimal PriceAmount { get; set; }
        public required string PriceCurrency { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDbModel? Category { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
