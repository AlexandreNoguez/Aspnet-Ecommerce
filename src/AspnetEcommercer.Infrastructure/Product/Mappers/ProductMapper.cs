using AspnetEcommerce.Domain.Product.Entity;
using AspnetEcommerce.Domain.Product.ValueObject;
using AspnetEcommerce.Infrastructure.Product.Models;

namespace AspnetEcommerce.Infrastructure.Product.Mappers;

public static class ProductMapper
{
    public static ProductDbModel ToDbModel(ProductEntity entity)
    {
        return new ProductDbModel
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            Description = entity.Description.Value,
            Sku = entity.Sku.Value,
            PriceAmount = entity.Price.Amount,
            PriceCurrency = entity.Price.Currency,
            CategoryId = entity.Category.Id,
            StockQuantity = entity.StockQuantity,
            IsActive = entity.IsActive
        };
    }

    public static ProductEntity ToDomain(ProductDbModel model, CategoryDbModel category)
    {
        var categoryEntity = CategoryMapper.ToDomain(category);

        return new ProductEntity(
            model.Id,
            ProductName.Create(model.Name),
            ProductDescription.Create(model.Description),
            Money.Create(model.PriceAmount, model.PriceCurrency),
            Sku.Create(model.Sku),
            categoryEntity,
            model.StockQuantity,
            model.IsActive);
    }

    public static void ApplyToDbModel(ProductEntity entity, ProductDbModel model)
    {
        model.Name = entity.Name.Value;
        model.Description = entity.Description.Value;
        model.Sku = entity.Sku.Value;
        model.PriceAmount = entity.Price.Amount;
        model.PriceCurrency = entity.Price.Currency;
        model.CategoryId = entity.Category.Id;
        model.StockQuantity = entity.StockQuantity;
        model.IsActive = entity.IsActive;
    }
}
