using Discount.Grpc.Protos;
using Discount.Service.Commands;
using Discount.Service.Data.DTOs;
using Discount.Service.Entities;

namespace Discount.Service.Mappers
{
    public static class DiscountMapper
    {
        public static CouponDto ToDto(this Coupon coupon)
        {
            return new CouponDto
            (coupon.Id, coupon.ProductName, coupon.Description, coupon.Amount
            );
        }

        public static Coupon ToEntity(this CreateDiscountCommand discountCommand)
        {
            return new Coupon
            {
                ProductName = discountCommand.ProductName,
                Description = discountCommand.Description,
                Amount = discountCommand.Amount
            };
        }


        public static Coupon ToEntity(this UpdateDiscountCommand discountCommand)
        {
            return new Coupon
            {
                Id = discountCommand.Id,
                ProductName = discountCommand.ProductName,
                Description = discountCommand.Description,
                Amount = discountCommand.Amount
            };
        }


        public static CouponModel ToModel(this CouponDto couponModel)
        {
            return new CouponModel
            {
                Id = couponModel.Id,
                ProductName = couponModel.ProductName,
                Description = couponModel.Description,
                Amount = couponModel.Amount
            };
        }

        public static CreateDiscountCommand ToCreateCommand(this CouponModel couponModel)
        {
            return new CreateDiscountCommand
            (
               couponModel.ProductName,
               couponModel.Description,
               couponModel.Amount
            );
        }

        public static UpdateDiscountCommand ToUpdateCommand(this CouponModel couponModel)
        {
            return new UpdateDiscountCommand
            (
               couponModel.Id,
               couponModel.ProductName,
               couponModel.Description,
               couponModel.Amount
            );
        }
    }
}
