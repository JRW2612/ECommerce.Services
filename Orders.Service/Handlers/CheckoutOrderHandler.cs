using MediatR;
using Orders.Service.Commands;
using Orders.Service.Mapper;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _oRepository;
        private readonly ILogger<CheckoutOrderHandler> _logger;

        public CheckoutOrderHandler(IOrderRepository oRepository, ILogger<CheckoutOrderHandler> logger)
        {
            _oRepository = oRepository;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = request.ToEntity();
            var generatedOrder = await _oRepository.AddAsync(orderEntity);

            if (generatedOrder is null)
            {
                _logger.LogWarning("Order creation failed.");
                return 0;
            }
            var outboxHandle = OrderMapper.ToOutBoxMessage(generatedOrder, request.CorrelationId);
            await _oRepository.AddOutboxMessageAsync(outboxHandle);
            _logger.LogInformation($"Order {generatedOrder.Id} is successfully created.");
            return generatedOrder.Id;
        }
    }
}
