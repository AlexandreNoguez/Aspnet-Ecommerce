using AspnetEcommercer.Infrastructure.Customer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetEcommercer.Infrastructure.Database.Mappings
{
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerDbModel>
    {
        public void Configure(EntityTypeBuilder<CustomerDbModel> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.IsActive)
                .IsRequired();
            builder.Property(c => c.RewardPoints)
                .IsRequired();
            // Flattened address columns
            //builder.OwnsOne(c => c.Address, address =>
            //{
            //    address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
            //    address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
            //    address.Property(a => a.State).HasColumnName("State").HasMaxLength(100);
            //    address.Property(a => a.ZipCode).HasColumnName("ZipCode").HasMaxLength(20);
            //    address.Property(a => a.Number).HasColumnName("Number");
            //});

            // Optional address fields
            builder.Property(x => x.ZipCode).HasMaxLength(20);
            builder.Property(x => x.State).HasMaxLength(2);
            builder.Property(x => x.City).HasMaxLength(120);
            builder.Property(x => x.Street).HasMaxLength(200);
            builder.Property(x => x.Number).HasMaxLength(6);
        }
    }
}
