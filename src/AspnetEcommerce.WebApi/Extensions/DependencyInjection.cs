using AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;
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
        // DbContext (Infra)
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        // Unit of Work (Infra)
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repository (Infra)
        services.AddScoped<ICustomerRepository, CustomerRepositoryEf>();

        // Use Cases (Application)
        //services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();

        return services;
    }
}
