using Basket.Service.Commands;
using Basket.Service.GrpcService;
using Basket.Service.Repositories.Interfaces;
using MediatR;

namespace Basket.Service.Handlers
{
    public class DeleteBasketByUserNameHander : IRequestHandler<DeleteBasketByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _bRepository;
        private readonly DiscountGrcpService _discountGrcpService;

        public DeleteBasketByUserNameHander(IBasketRepository bRepository, DiscountGrcpService discountGrcpService)
        {
            _bRepository = bRepository;
            _discountGrcpService = discountGrcpService;
        }
        public async Task<Unit> Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _bRepository.DeleteBasketAsync(request.userName);
            return Unit.Value;
        }
    }
}
