using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AspnetEcommerce.Infrastructure.Database
{
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private const string ConnectionName = "PostgresConnection";

        public DatabaseContext CreateDbContext(string[] args)
        {
            var fallbackConnectionString =
                "Host=localhost;Port=5432;Database=ecommerce;Username=ecommerce;Password=ecommerce;";

            var configuration = new ConfigurationBuilder()
                // Fallback default values (works locally even without env vars)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [$"ConnectionStrings:{ConnectionName}"] = fallbackConnectionString,

                    // Optional: support a simpler env key like App_PostgresConnection
                    [ConnectionName] = fallbackConnectionString
                })
                // Reads env vars with prefix "App_"
                // Example:
                //   App_ConnectionStrings__PostgresConnection="Host=...;Port=...;"
                // or
                //   App_PostgresConnection="Host=...;Port=...;"
                .AddEnvironmentVariables(prefix: "App_")
                .Build();

            // Prefer standard .NET connection string section first
            var connectionString =
                configuration.GetConnectionString(ConnectionName)
                ?? configuration[ConnectionName];

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    $"Connection string '{ConnectionName}' was not found. " +
                    $"Set 'App_ConnectionStrings__{ConnectionName}' (recommended) or 'App_{ConnectionName}'.");

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}


//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace AspnetEcommerce.Infrastructure.Database
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
//    {
//        public DatabaseContext CreateDbContext(string[] args)
//        {
//            var configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(new Dictionary<string, string>
//                {
//                    ["ConnectionStrings:PostgresConnection"] = "Host=localhost;Port=5432;Database=ecommerce;Username=ecommerce;Password=ecommerce;"
//                })
//                .AddEnvironmentVariables("App_")
//                .Build();

//            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
//            var connectionString = configuration.GetConnectionString("PostgresConnection");
//            optionsBuilder.UseNpgsql(connectionString);

//            return new DatabaseContext(optionsBuilder.Options);
//        }
//    }
//}
