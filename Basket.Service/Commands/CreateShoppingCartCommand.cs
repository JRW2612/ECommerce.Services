using Basket.Service.Data.DTOs;
using Basket.Service.Responses;
using MediatR;

namespace Basket.Service.Commands
{
    public record CreateShoppingCartCommand(string userName, List<CreateShoppingCartItemDto> Items) : IRequest<ShoppingCartResponse>
    {
    }
}
