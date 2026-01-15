using AspnetEcommerce.Infrastructure.Product.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetEcommerce.Infrastructure.Database.Mappings
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductDbModel>
    {
        public void Configure(EntityTypeBuilder<ProductDbModel> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Sku).IsUnique();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PriceAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.PriceCurrency)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(p => p.StockQuantity)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
