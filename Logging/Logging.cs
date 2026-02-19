using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;
                loggerConfiguration
                    .MinimumLevel.Information()
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                    .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                    .WriteTo.Console();

                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Orders", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Identity", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Payments", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                }

                //ElasticSearch
                var elasticUri = context.Configuration.GetValue<string>("ElasticSearchConfig:Uri");
                if (!string.IsNullOrEmpty(elasticUri))
                {
                    loggerConfiguration.WriteTo
                    .Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                            IndexFormat = "ecommerce-log-{0:yyyy-MM-dd}",
                            MinimumLogEventLevel = LogEventLevel.Debug
                        });
                }
            };

    }
}
