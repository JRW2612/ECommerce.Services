using MediatR;

namespace Discount.Service.Commands
{
    public record DeleteDiscountCommand(string ProductName) : IRequest<bool>
    {

    }
}
