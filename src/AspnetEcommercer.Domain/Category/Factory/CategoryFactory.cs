using AspnetEcommerce.Domain.Category.Entity;

namespace AspnetEcommerce.Domain.Category.Factory
{
    public static class CategoryFactory
    {
        public static CategoryEntity CreateNewCategory(string name, string slug, string? description)
        {
            return new CategoryEntity(Guid.NewGuid(), name, slug, description, true, 0);
        }
    }
}
