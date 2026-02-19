using EventBus.Messages.Events;
using MassTransit;
using Orders.Service.Entities;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.EvenrOrderConsumer
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(IOrderRepository orderRepository, ILogger<PaymentFailedConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            if (order is null)
            {
                _logger.LogError("Order with Id {OrderId} and {correlationId} not found", context.Message.OrderId, context.Message.CorrelationId);
                return;
            }
            order.Status = OrderStatus.Failed;
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation("Order with Id {OrderId} marked as Failed due to payment failure. Reason: {Reason}", context.Message.OrderId, context.Message.Reason);

        }
    }
}
