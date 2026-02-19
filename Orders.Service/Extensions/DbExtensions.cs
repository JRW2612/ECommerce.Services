using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Orders.Service.Extensions
{
    public static class DbExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
      where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();
                try
                {
                    logger.LogInformation($"Started Migration for database using context {typeof(TContext).Name}");
                    var retry = Policy.Handle<SqlException>().
                        WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, span, count) =>
                        {
                            logger.LogInformation($"Retrying beacause of {exception},{span}");
                        });
                    retry.Execute(() => CallSeeder(seeder, context, services));
                    logger.LogInformation($"Migration completed for database using context {typeof(TContext).Name}");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database using context {typeof(TContext).Name}");
                    throw;
                }
            }
            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
