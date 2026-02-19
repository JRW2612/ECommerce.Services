using MediatR;
using Orders.Service.Commands;
using Orders.Service.Entities;
using Orders.Service.Exceptions;
using Orders.Service.Mapper;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _oRepository;
        private readonly ILogger<CheckoutOrderHandler> _logger;

        public UpdateOrderHandler(IOrderRepository oRepository, ILogger<CheckoutOrderHandler> logger)
        {
            _oRepository = oRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _oRepository.GetByIdAsync(request.Id);

            if (orderToUpdate is null)
            {
                _logger.LogWarning($"Order Updation failed for {request.Id}.");
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            orderToUpdate.MapUpdate(request);
            var outboxMessage = OrderMapper.ToOutBoxMessageForUpdate(orderToUpdate, request.CorrelationId);
            await _oRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation($"Order Updated successfully for {request.Id}.");
            return Unit.Value;
        }
    }
}
