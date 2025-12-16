using AspnetEcommerce.Infrastructure.Customer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetEcommerce.Infrastructure.Database.Mappings
{
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerDbModel>
    {
        public void Configure(EntityTypeBuilder<CustomerDbModel> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IsActive)
                .IsRequired();
            builder.Property(c => c.RewardPoints)
                .IsRequired();
            builder.Property(c => c.ZipCode).HasMaxLength(20);
            builder.Property(c => c.State).HasMaxLength(2);
            builder.Property(c => c.City).HasMaxLength(120);
            builder.Property(c => c.Street).HasMaxLength(200);
            builder.Property(c => c.Number);
        }
    }
}
