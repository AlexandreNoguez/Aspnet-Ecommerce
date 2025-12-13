using AspnetEcommercer.Infrastructure.Customer.Models;
using AspnetEcommercer.Infrastructure.Database.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommercer.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //public DbSet<CustomerDbModel> Customers { get; set; }
        public DbSet<CustomerDbModel> Customers => Set<CustomerDbModel>();
        //public DbSet<AddressDbModel> Addresses => Set<AddressDbModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
