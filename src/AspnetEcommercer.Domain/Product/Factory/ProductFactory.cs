using AspnetEcommerce.Domain.Category.Entity;
using AspnetEcommerce.Domain.Product.Entity;
using AspnetEcommerce.Domain.Product.ValueObject;

namespace AspnetEcommerce.Domain.Product.Factory
{
    public static class ProductFactory
    {
        public static ProductEntity CreateNewProduct(string name,
            string description,
            decimal amount,
            string currency,
            string sku,
            CategoryEntity category,
            int stockQuantity)
        {
            var productName = ProductName.Create(name);
            var productDescription = ProductDescription.Create(description);
            var price = Money.Create(amount, currency);
            var productSku = Sku.Create(sku);

            return new ProductEntity(Guid.NewGuid(), productName, productDescription, price, productSku, category, stockQuantity, true);
        }
    }
}
