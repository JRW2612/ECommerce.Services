using Discount.Service.Entities;

namespace Discount.Service.Repositories.Interface
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string ProductName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);

        Task<bool> DeleteDiscount(string ProductName);
    }
}
