using MediatR;

namespace Basket.Service.Commands
{
    public record DeleteBasketByUserNameCommand(string userName) : IRequest<Unit>
    {

    }
}
