using Basket.Service.Commands;
using Basket.Service.GrpcService;
using Basket.Service.Mappers;
using Basket.Service.Repositories.Interfaces;
using Basket.Service.Responses;
using MediatR;

namespace Basket.Service.Handlers
{
    public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _bRepository;
        private readonly DiscountGrcpService _discountGrcpService;

        public CreateShoppingCartHandler(IBasketRepository bRepository, DiscountGrcpService discountGrcpService)
        {
            _bRepository = bRepository;
            _discountGrcpService = discountGrcpService;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            //Apply discount using grpc call 
            foreach (var discountItem in request.Items)
            {
                var coupon = await _discountGrcpService.GetDiscountAsync(discountItem.ProductName);
                discountItem.Price -= coupon.Amount;
            }
            // Convert Command to Entity
            var cartEntity = request.ToEntity();

            // Save Entity to Redis
            var redisCart = await _bRepository.UpsertBasketAsync(cartEntity);

            // Convert Entity to Response
            var response = redisCart.ToResponse();

            return response;
        }
    }
}
