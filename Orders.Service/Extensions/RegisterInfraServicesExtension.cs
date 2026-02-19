using Microsoft.EntityFrameworkCore;
using Orders.Service.Data;
using Orders.Service.Repositories;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.Extensions
{
    public static class RegisterInfraServicesExtension
    {
        public static IServiceCollection RegisterInfraServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<OrderContext>(sql => sql.UseSqlServer(
                configuration.GetConnectionString("OrderConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            return services;
        }
    }
}
