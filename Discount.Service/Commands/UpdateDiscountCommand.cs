using Discount.Service.Data.DTOs;
using MediatR;

namespace Discount.Service.Commands
{
    public record UpdateDiscountCommand(int Id, string ProductName, string Description, int Amount) : IRequest<CouponDto>
    {
    }
}
