using Catalog.Service.Repositories;
using Catalog.Service.Repositories.Interfaces;

namespace Catalog.Service.Helpers
{
    public static class RegisterServiceHelper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration? configuration)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();


            return services;
        }


    }
}
