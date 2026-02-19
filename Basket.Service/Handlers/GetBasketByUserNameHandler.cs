using Basket.Service.GrpcService;
using Basket.Service.Mappers;
using Basket.Service.Queries;
using Basket.Service.Repositories.Interfaces;
using Basket.Service.Responses;
using MediatR;

namespace Basket.Service.Handlers
{
    public class GetBasketByUserNameHandler : IRequestHandler<GetBasketByUsernameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _bRepository;
        private readonly DiscountGrcpService _discountGrcpService;

        public GetBasketByUserNameHandler(IBasketRepository bRepository, DiscountGrcpService discountGrcpService)
        {
            _bRepository = bRepository;
            _discountGrcpService = discountGrcpService;
        }
        public async Task<ShoppingCartResponse> Handle(GetBasketByUsernameQuery request, CancellationToken cancellationToken)
        {


            var shoppingCart = await _bRepository.GetBasketAsync(request.userName);

            if (shoppingCart is null)
            {
                return new ShoppingCartResponse(request.userName)
                {
                    Items = new List<ShoppingCartItemsResponse>()
                };
            }
            return shoppingCart.ToResponse();
        }
    }
}
