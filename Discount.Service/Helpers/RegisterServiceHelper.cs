using Discount.Service.Repositories;
using Discount.Service.Repositories.Interface;

namespace Discount.Service.Helpers
{
    public static class RegisterServiceHelper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration? configuration)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddGrpc();
            return services;
        }
    }
}
