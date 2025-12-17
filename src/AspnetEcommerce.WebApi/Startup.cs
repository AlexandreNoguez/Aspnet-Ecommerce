//using AspnetEcommerce.Domain.Contracts.Abstractions;
//using AspnetEcommerce.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Mvc;

//[assembly: ApiController]
//namespace AspnetEcommerce.WebApi
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration) {
//            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            // Other service configurations...
//            // Register the UnitOfWork
//            services.AddScoped<IUnitOfWork, UnitOfWork>();
//            //services.AddRepositories(Configuration);
//            // Other service configurations...
//        }
//    }
//}
