using AspnetEcommerce.Infrastructure.Product.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetEcommerce.Infrastructure.Database.Mappings
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryDbModel>
    {
        public void Configure(EntityTypeBuilder<CategoryDbModel> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Slug).IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.IsActive)
                .IsRequired();

            builder.Property(c => c.DisplayOrder)
                .IsRequired();
        }
    }
}
