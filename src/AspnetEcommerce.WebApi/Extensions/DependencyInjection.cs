using AspnetEcommerce.Application.Contracts.Email;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using AspnetEcommerce.Application.Customer.Contracts.Links;
using AspnetEcommerce.Application.Customer.UseCases.ActivateCustomer;
using AspnetEcommerce.Application.Customer.UseCases.CreateCustomer;
using AspnetEcommerce.Application.Customer.UseCases.DeleteCustomer;
using AspnetEcommerce.Application.Customer.UseCases.GetAllCustomers;
using AspnetEcommerce.Application.Customer.UseCases.GetCustomerByIdUseCase;
using AspnetEcommerce.Application.Customer.UseCases.SoftDeleteCustomer;
using AspnetEcommerce.Application.Customer.UseCases.UpdateCustomer;
using AspnetEcommerce.Application.Product.UseCases.CreateCategory;
using AspnetEcommerce.Application.Product.UseCases.CreateProduct;
using AspnetEcommerce.Application.Product.UseCases.DeleteCategory;
using AspnetEcommerce.Application.Product.UseCases.DeleteProduct;
using AspnetEcommerce.Application.Product.UseCases.GetAllCategories;
using AspnetEcommerce.Application.Product.UseCases.GetAllProducts;
using AspnetEcommerce.Application.Product.UseCases.GetCategoryById;
using AspnetEcommerce.Application.Product.UseCases.GetProductById;
using AspnetEcommerce.Application.Product.UseCases.SoftDeleteCategory;
using AspnetEcommerce.Application.Product.UseCases.SoftDeleteProduct;
using AspnetEcommerce.Application.Product.UseCases.UpdateCategory;
using AspnetEcommerce.Application.Product.UseCases.UpdateProduct;
using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Domain.Customer.Activation;
using AspnetEcommerce.Domain.Customer.Repository;
using AspnetEcommerce.Domain.Product.Repository;
using AspnetEcommerce.Infrastructure.Customer.Repository;
using AspnetEcommerce.Infrastructure.Database;
using AspnetEcommerce.Infrastructure.Email.SmtpConfig;
using AspnetEcommerce.Infrastructure.Email.Templates.CustomerWelcome;
using AspnetEcommerce.Infrastructure.Messaging;
using AspnetEcommerce.Infrastructure.Persistence;
using AspnetEcommerce.Infrastructure.Product.Repository;
using AspnetEcommerce.WebApi.Infra.Links;
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
        services.AddScoped<IProductRepository, ProductRepositoryEf>();
        services.AddScoped<ICategoryRepository, CategoryRepositoryEf>();

        // RabbitMQ options + fila
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IEmailJobQueue, RabbitMqEmailJobQueue>();

        // SMTP + template (usados pelo worker)
        services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ICustomerEmailTemplateProvider, CustomerEmailTemplateProvider>();

        // Activation link builder
        services.AddScoped<IActivationLinkBuilder, ConfigurationActivationLinkBuilder>();

        // Activation token repository
        services.AddScoped<ICustomerActivationTokenRepository, CustomerActivationTokenRepositoryEf>();

        // Worker para consumir fila de e-mails
        services.AddHostedService<WelcomeEmailWorker>();

        // Use cases
        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
        services.AddScoped<IActivateCustomerUseCase, ActivateCustomerUseCase>();
        services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCase>();
        services.AddScoped<IGetCustomerByIdUseCase, GetCustomerByIdUseCase>();
        services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
        services.AddScoped<IDeleteCustomerUseCase, DeleteCustomerUseCase>();
        services.AddScoped<ISoftDeleteCustomerUseCase, SoftDeleteCustomerUseCase>();

        // Products and Categories use cases would be registered here similarly
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
        services.AddScoped<ISoftDeleteProductUseCase, SoftDeleteProductUseCase>();
        services.AddScoped<ISoftDeleteCategoryUseCase, SoftDeleteCategoryUseCase>();

        return services;
    }
}