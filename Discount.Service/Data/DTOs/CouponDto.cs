namespace Discount.Service.Data.DTOs
{
    public record CouponDto(
             int Id,
             string ProductName,
             string Description,
            int Amount
        );

}
