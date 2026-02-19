using Npgsql;

namespace Discount.Service.Extensions
{
    public static class DiscountDbExtension
    {
        public static IHost MigrateDataBase<IContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<IContext>>();
                try
                {
                    logger.LogInformation("Migrating DiscountDB database started.");
                    ApplyMigration(config); // Pass the required IConfiguration argument
                    logger.LogInformation("Migrating DiscountDB database done.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the DiscountDB database.");
                    throw;
                }
            }
            return host;
        }

        private static void ApplyMigration(IConfiguration configuration)
        {
            int retry = 0;
            const int maxRetries = 5;

            while (retry < maxRetries)
            {
                //         using var conn = new NpgsqlConnection(
                //configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                //         conn.Open();
                using var conn = new NpgsqlConnection(configuration["DatabaseSettings:ConnectionString"]
);
                conn.Open();

                try
                {
                    using var cmd = new NpgsqlCommand();
                    cmd.Connection = conn;

                    // Fix: Correct SQL Syntax
                    cmd.CommandText = "DROP TABLE IF EXISTS Coupon;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
                        CREATE TABLE Coupon(
                            Id SERIAL PRIMARY KEY,
                            ProductName VARCHAR(24) NOT NULL,
                            Description TEXT,
                            Amount INT
                        );";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
                        INSERT INTO Coupon(ProductName, Description, Amount)
                        VALUES 
                            ('Babolat Apparel 19', 'High-quality apparel by Babolat designed for professionals and enthusiasts alike.', 997),
                            ('Adidas Shuttlecocks 20', 'High-quality shuttlecocks by Adidas designed for professionals and enthusiasts alike.', 685),
                            ('Babolat Shoes 21', 'High-quality shoes by Babolat designed for professionals and enthusiasts alike.', 780),
                            ('Puma Shoes 22', 'High-quality shoes by Puma designed for professionals and enthusiasts alike.', 438),
                            ('Asics Shoes 23', 'High-quality shoes by Asics designed for professionals and enthusiasts alike.', 944);";

                    cmd.ExecuteNonQuery();

                    // If all good 
                    conn.Close();
                }
                catch (NpgsqlException)
                {
                    retry++;

                    if (retry >= maxRetries)
                        throw;

                    Thread.Sleep(2000); // wait 2 seconds before retry
                }
            }

        }

    }
}
