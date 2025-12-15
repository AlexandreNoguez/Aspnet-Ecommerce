using AspnetEcommercer.Domain.Contracts.Abstractions;
using AspnetEcommercer.Domain.Customer.Repository;
using AspnetEcommercer.Infrastructure.Customer.Repository;
using AspnetEcommercer.Infrastructure.Database;
using AspnetEcommercer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AspnetEcommerce.WebApi.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Presentation
        //services.AddScoped<CreateCustomerUseCase>();
        //services.AddScoped<GetCustomerUseCase>();

        return services;
    }
}
