namespace Discount.Service.Responses
{
    public class Responses
    {
        public class CouponResponse
        {
            public int Id { get; init; }
            public string? ProductName { get; init; }
            public string? Description { get; init; }
            public int Amount { get; init; }
        }
    }
}
