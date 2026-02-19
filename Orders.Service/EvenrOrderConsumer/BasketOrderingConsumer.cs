using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using Orders.Service.Mapper;

namespace Orders.Service.EvenrOrderConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketOrderingConsumer> _logger;

        public BasketOrderingConsumer(IMediator mediator, ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("BasketCheckoutEvent Consumer Scope for {correlationId}", context.Message.CorrelationId);
            var command = context.Message.ToCheckoutOrderCommand();
            await _mediator.Send(command);
            _logger.LogInformation("Basket Checkout Event completed successfully.");
        }
    }
}
