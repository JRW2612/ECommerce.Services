using MediatR;
using Orders.Service.Commands;
using Orders.Service.Entities;
using Orders.Service.Exceptions;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _oRepository;
        private readonly ILogger<CheckoutOrderHandler> _logger;

        public DeleteOrderHandler(IOrderRepository oRepository, ILogger<CheckoutOrderHandler> logger)
        {
            _oRepository = oRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _oRepository.GetByIdAsync(request.Id);

            if (orderToDelete is null)
            {
                _logger.LogWarning($"Order not found for {request.Id}.");
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            await _oRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order {request.Id} is successfully deleted.");
            return Unit.Value;
        }
    }
}
