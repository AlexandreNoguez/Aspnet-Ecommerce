using AspnetEcommerce.Application.Contracts.Email;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Infrastructure.Customer.Repository;
using AspnetEcommerce.Infrastructure.Database;
using AspnetEcommerce.Infrastructure.Email.SmtpConfig;
using AspnetEcommerce.Infrastructure.Email.Templates.CustomerWelcome;
using AspnetEcommerce.Infrastructure.Messaging;
using AspnetEcommerce.Infrastructure.Persistence;
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

        // RabbitMQ options + fila
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IEmailJobQueue, RabbitMqEmailJobQueue>();

        // SMTP + template (usados pelo worker)
        services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ICustomerEmailTemplateProvider, CustomerEmailTemplateProvider>();

        // Worker para consumir fila de e-mails
        services.AddHostedService<WelcomeEmailWorker>();

        // Use cases
        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();

        return services;
    }
}
