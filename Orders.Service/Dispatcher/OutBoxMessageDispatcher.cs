using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Orders.Service.Data;

namespace Orders.Service.Dispatcher
{
    public class OutBoxMessageDispatcher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutBoxMessageDispatcher> _logger;

        public OutBoxMessageDispatcher(IServiceProvider serviceProvider, ILogger<OutBoxMessageDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    var outboxService = scope.ServiceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();

                    var publishEndPoints = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var pendingMessages = await dbContext.OutBoxMessages
                        .Where(m => !m.IsProcessed || m.IsProcessed == null)
                        .OrderBy(m => m.OccurredOn)
                        .Take(20)
                        .ToListAsync();
                    foreach (var message in pendingMessages)
                    {
                        try
                        {
                            var dynamicData = JsonConvert.DeserializeObject<dynamic>(message.Content);
                            var ordercreatedEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(Convert.ToString(dynamicData));
                            await publishEndPoints.Publish(ordercreatedEvent);
                            message.ProcessedOn = DateTime.UtcNow;
                            _logger.LogInformation($"Publishing outbox message {message.Id}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex, $"Failed outbox message {message.Id}");
                            throw;
                        }
                    }
                }
            }

        }
    }
}
