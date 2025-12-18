using AspnetEcommerce.Infrastructure.Customer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspnetEcommerce.Infrastructure.Database.Mappings
{
    public class CustomerActivationTokenConfiguration
        : IEntityTypeConfiguration<CustomerActivationTokenDbModel>
    {
        public void Configure(EntityTypeBuilder<CustomerActivationTokenDbModel> builder)
        {
            builder.ToTable("CustomerActivationTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.UsedAt);

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.HasIndex(x => x.CustomerId);
        }
    }
}
