using EventBus.Messages.Events;
using MassTransit;
using Orders.Service.Entities;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.EvenrOrderConsumer
{
    public class PaymentCompletedConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentCompletedConsumer> _logger;

        public PaymentCompletedConsumer(IOrderRepository orderRepository, ILogger<PaymentCompletedConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            if (order is null)
            {
                _logger.LogError("Order with Id {OrderId} and {correlationId} not found", context.Message.OrderId, context.Message.CorrelationId);
            }
            order.Status = OrderStatus.Paid;
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation("Order with Id {OrderId} has been marked as Paid", context.Message.OrderId);
        }
    }
}
