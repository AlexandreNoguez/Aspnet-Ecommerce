using AspnetEcommerce.Domain.Category.Entity;
using AspnetEcommerce.Domain.Product.ValueObject;

namespace AspnetEcommerce.Domain.Product.Entity
{
    public class ProductEntity
    {
        public ProductEntity(Guid id,
            ProductName name,
            ProductDescription description,
            Money price,
            Sku sku,
            CategoryEntity category,
            int stockQuantity,
            bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Sku = sku;
            Category = category;
            SetStockQuantity(stockQuantity);
            IsActive = isActive;
            Validate();
        }

        public Guid Id { get; private set; }
        public ProductName Name { get; private set; }
        public ProductDescription Description { get; private set; }
        public Money Price { get; private set; }
        public Sku Sku { get; private set; }
        public CategoryEntity Category { get; private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; }

        public static ProductEntity Create(Guid id,
            ProductName name,
            ProductDescription description,
            Money price,
            Sku sku,
            CategoryEntity category,
            int stockQuantity)
        {
            return new ProductEntity(id, name, description, price, sku, category, stockQuantity, true);
        }

        public void Activate()
        {
            if (StockQuantity <= 0)
            {
                throw new Exception("Stock must be greater than zero to activate a product");
            }

            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void ChangePrice(Money price)
        {
            Price = price ?? throw new ArgumentNullException(nameof(price));
            Validate();
        }

        public void ChangeCategory(CategoryEntity category)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Validate();
        }

        public void ChangeName(ProductName name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Validate();
        }

        public void ChangeDescription(ProductDescription description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Validate();
        }

        public void ChangeSku(Sku sku)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku));
            Validate();
        }

        public void SetStockQuantity(int stockQuantity)
        {
            if (stockQuantity < 0)
            {
                throw new Exception("Stock quantity cannot be negative");
            }

            StockQuantity = stockQuantity;
        }

        private void Validate()
        {
            if (Id == Guid.Empty)
            {
                throw new Exception("Id is required");
            }

            if (Category is null)
            {
                throw new Exception("Category is required");
            }
        }
    }
}
