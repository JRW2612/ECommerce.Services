using Basket.Service.Commands;
using Basket.Service.Mappers;
using Basket.Service.Queries;
using MassTransit;
using MediatR;

namespace Basket.Service.Handlers
{
    public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private ILogger<CheckoutBasketCommandHandler> _logger;

        public CheckoutBasketCommandHandler(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<CheckoutBasketCommandHandler> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task<Unit> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            var Dto = request.Dto;

            var basketResponse = await _mediator.Send(new GetBasketByUsernameQuery(Dto.UserName), cancellationToken);
            if (basketResponse is null || !basketResponse.Items.Any())
            {
                _logger.LogWarning("Basket is empty for user {UserName}", Dto.UserName);
            }

            var basketData = basketResponse.ToEntity();
            //Map to event
            var et = Dto.ToCheckoutBasketEvent(basketData);
            _logger.LogInformation("Publishing checkout event for user {UserName}", Dto.UserName);
            await _publishEndpoint.Publish(et, cancellationToken);
            //delete basket
            await _mediator.Send(new DeleteBasketByUserNameCommand(Dto.UserName), cancellationToken);
            return Unit.Value;
        }
    }
}
