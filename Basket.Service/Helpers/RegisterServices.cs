using Basket.Service.GrpcService;
using Basket.Service.Repositories;
using Basket.Service.Repositories.Interfaces;
using Discount.Grpc.Protos;
using MassTransit;

namespace Basket.Service.Helpers
{
    public static class RegisterServices
    {
        public static IServiceCollection MyServices(this IServiceCollection services, IConfiguration? configuration)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<DiscountGrcpService>();
            services.AddGrpc();
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                cfg => cfg.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));
            services.AddMassTransit(configure =>
            {
                configure.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                });
            });
            return services;
        }
    }
}
