using Discount.Service.Entities;
using Discount.Service.Repositories.Interface;
using Npgsql;

namespace Discount.Service.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string? _connection;

        public DiscountRepository(IConfiguration configuration)
        {
            _connection = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            var affected = new NpgsqlCommand
            {
                Connection = conn,
                CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount);"
            };
            affected.Parameters.AddWithValue("ProductName", coupon.ProductName);
            affected.Parameters.AddWithValue("Description", coupon.Description);
            affected.Parameters.AddWithValue("Amount", coupon.Amount);
            var result = await affected.ExecuteNonQueryAsync();
            await conn.CloseAsync();
            return result != 0;
        }

        public async Task<bool> DeleteDiscount(string ProductName)
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            var deleted = new NpgsqlCommand
            {
                Connection = conn,
                CommandText = "DELETE FROM Coupon WHERE ProductName=@ProductName;"
            };
            deleted.Parameters.AddWithValue("ProductName", ProductName);
            var result = await deleted.ExecuteNonQueryAsync();
            await conn.CloseAsync();
            return result != 0;
        }

        public async Task<Coupon> GetDiscount(string ProductName)
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();

            // Fix for CS1061: Use NpgsqlCommand and read manually, since QueryFirstOrDefaultAsync is not available.
            using var cmd = new NpgsqlCommand("SELECT * FROM Coupon WHERE ProductName=@ProductName;", conn);
            cmd.Parameters.AddWithValue("ProductName", ProductName);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var coupon = new Coupon
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    ProductName = reader["ProductName"].ToString(),
                    Description = reader["Description"].ToString(),
                    Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
                };
                await conn.CloseAsync();
                return coupon ?? new Coupon
                {
                    ProductName = $"No Discount founf for '" + coupon.ProductName + "'",
                    Amount = 0,
                    Description = $"No information available"
                };
            }
            await conn.CloseAsync();
            return null;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            var updated = new NpgsqlCommand
            {
                Connection = conn,
                CommandText = "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id;"
            };
            // updated.Parameters.AddWithValue("Id", coupon.Id);
            updated.Parameters.AddWithValue("ProductName", coupon.ProductName);
            updated.Parameters.AddWithValue("Description", coupon.Description);
            updated.Parameters.AddWithValue("Amount", coupon.Amount);
            var result = await updated.ExecuteNonQueryAsync();
            await conn.CloseAsync();
            return result != 0;
        }
    }
}
