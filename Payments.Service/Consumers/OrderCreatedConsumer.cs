using EventBus.Messages.Events;
using MassTransit;

namespace Payments.Service.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderCreatedEvent> _logger;

        public OrderCreatedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedEvent> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation($"Processing payment for OrderId:{message.Id}");
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", message.CorrelationId))
            {
                await Task.Delay(1000);
                if (message.TotalPrice > 0)
                {
                    var completedEvent = new PaymentCompletedEvent
                    {
                        OrderId = message.Id,
                        CorrelationId = message.CorrelationId,
                        TotalPrice = message.TotalPrice,
                        UserName = message.UserName,
                        TimeStamp = DateTime.UtcNow
                    };
                    await _publishEndpoint.Publish(completedEvent);
                    _logger.LogInformation($"Payment processed successfully for OrderId:{message.Id} and CorrelationId:{context.CorrelationId.Value}");

                }
                else
                {
                    var failedEvent = new PaymentFailedEvent
                    {
                        OrderId = message.Id,
                        CorrelationId = message.CorrelationId,
                        Reason = "Payment could not zero or negative amount",
                        TimeStamp = DateTime.UtcNow
                    };
                    await _publishEndpoint.Publish(failedEvent);
                    _logger.LogInformation($"Payment processed unsuccessfully for OrderId:{message.Id} and CorrelationId:{context.CorrelationId.Value}");

                }
            }
        }
    }
}
