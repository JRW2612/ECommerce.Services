using Discount.Service.Data.DTOs;
using MediatR;

namespace Discount.Service.Queries
{
    public record GetDiscountQuery(string ProductName) : IRequest<CouponDto>;
}
