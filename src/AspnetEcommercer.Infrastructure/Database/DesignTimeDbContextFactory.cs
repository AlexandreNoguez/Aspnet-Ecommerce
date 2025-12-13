using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspnetEcommercer.Infrastructure.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:PostgresConnection"] = "Host=localhost;Port=5432;Database=ecommerce;Username=ecommerce;Password=ecommerce;"
                })
                .AddEnvironmentVariables("App_")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}