namespace AspnetEcommerce.Domain.Category.Entity
{
    public class CategoryEntity
    {
        public CategoryEntity(Guid id, string name, string slug, string? description, bool isActive, int displayOrder)
        {
            Id = id;
            SetName(name);
            SetSlug(slug);
            SetDescription(description);
            SetDisplayOrder(displayOrder);
            IsActive = isActive;
            Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Slug { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public int DisplayOrder { get; private set; }

        public static CategoryEntity Create(Guid id, string name, string slug, string? description)
        {
            return new CategoryEntity(id, name, slug, description, true, 0);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name is required.", nameof(name));
            }

            if (name.Length > 100)
            {
                throw new ArgumentException("Category name must be 100 characters or less.", nameof(name));
            }

            Name = name.Trim();
        }

        public void SetSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Category slug is required.", nameof(slug));
            }

            if (slug.Length > 120)
            {
                throw new ArgumentException("Category slug must be 120 characters or less.", nameof(slug));
            }

            Slug = slug.Trim().ToLowerInvariant();
        }

        public void SetDescription(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
            {
                throw new ArgumentException("Category description must be 500 characters or less.", nameof(description));
            }

            Description = description?.Trim();
        }

        public void SetDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
            {
                throw new ArgumentException("Display order cannot be negative.", nameof(displayOrder));
            }

            DisplayOrder = displayOrder;
        }

        private void Validate()
        {
            if (Id == Guid.Empty)
            {
                throw new Exception("Id is required");
            }
        }
    }
}
