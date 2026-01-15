using AspnetEcommerce.Infrastructure.Customer.Models;
using AspnetEcommerce.Infrastructure.Database.Mappings;
using AspnetEcommerce.Infrastructure.Product.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //public DbSet<CustomerDbModel> Customers { get; set; }
        public DbSet<CustomerDbModel> Customers => Set<CustomerDbModel>();
        public DbSet<CustomerActivationTokenDbModel> CustomerActivationTokens => Set<CustomerActivationTokenDbModel>();
        public DbSet<ProductDbModel> Products => Set<ProductDbModel>();
        public DbSet<CategoryDbModel> Categories => Set<CategoryDbModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerActivationTokenConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
