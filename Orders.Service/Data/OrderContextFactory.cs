using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Orders.Service.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = configuration.GetConnectionString("Orderconnection");

            var optionsbuilder = new DbContextOptionsBuilder<OrderContext>();

            optionsbuilder.UseSqlServer(connection);

            return new OrderContext(optionsbuilder.Options);
        }
    }
}
