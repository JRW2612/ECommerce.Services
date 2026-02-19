using Basket.Service.Data.DTOs;
using MediatR;

namespace Basket.Service.Commands
{
    public record CheckoutBasketCommand(CheckoutBasketDto Dto) : IRequest<Unit>;
}
