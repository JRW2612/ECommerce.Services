using Orders.Service.Entities;

namespace Orders.Service.Data
{
    public class OrderContextSeedData
    {
        public static async Task SeedDataAsync(OrderContext context, ILogger<OrderContext> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetOrders());
                await context.SaveChangesAsync();
                logger.LogInformation($"Seeded Order data for {context}.");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "johndoe",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "johndoe@gmail.com",
                    AddressLine = "123 Main St",
                    Country = "USA",
                    State = "NY",
                    ZipCode = "10001",
                    TotalPrice = 250,
                    CardName = "John Doe",
                    CardNumber = "4111111111111111",
                    CVV = "123",
                    ExpirationDate = new DateTime(2028, 11, 1),
                    PaymentMethod =1,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow,



                }
            };
        }
    }
}
