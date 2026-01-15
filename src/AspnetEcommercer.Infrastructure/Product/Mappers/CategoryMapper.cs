using AspnetEcommerce.Domain.Category.Entity;
using AspnetEcommerce.Infrastructure.Product.Models;

namespace AspnetEcommerce.Infrastructure.Product.Mappers;

public static class CategoryMapper
{
    public static CategoryDbModel ToDbModel(CategoryEntity entity)
    {
        return new CategoryDbModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            IsActive = entity.IsActive,
            DisplayOrder = entity.DisplayOrder
        };
    }

    public static CategoryEntity ToDomain(CategoryDbModel model)
    {
        return new CategoryEntity(
            model.Id,
            model.Name,
            model.Slug,
            model.Description,
            model.IsActive,
            model.DisplayOrder);
    }

    public static void ApplyToDbModel(CategoryEntity entity, CategoryDbModel model)
    {
        model.Name = entity.Name;
        model.Slug = entity.Slug;
        model.Description = entity.Description;
        model.IsActive = entity.IsActive;
        model.DisplayOrder = entity.DisplayOrder;
    }
}
