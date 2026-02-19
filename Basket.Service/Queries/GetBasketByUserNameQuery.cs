using Basket.Service.Responses;
using MediatR;

namespace Basket.Service.Queries
{
    public record GetBasketByUsernameQuery(string userName) : IRequest<ShoppingCartResponse>;

}
