using EventBus.Messages.Common;
using MassTransit;
using MediatR;
using Orders.Service.Behaviour;
using Orders.Service.EvenrOrderConsumer;
using Orders.Service.Handlers;
using System.Reflection;

namespace Orders.Service.Extensions
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //Add swagger services
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<BasketOrderingConsumer>();

                configure.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
                    });
                });
            });
            //Register Mediatr Service
            var assemlblies = new Assembly[]
            {
    Assembly.GetExecutingAssembly(),
    typeof(CheckoutOrderHandler).Assembly
            };
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemlblies));

            return services;
        }
    }
}
