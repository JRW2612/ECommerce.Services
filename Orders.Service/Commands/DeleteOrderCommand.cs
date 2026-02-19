using MediatR;

namespace Orders.Service.Commands
{
    public record DeleteOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
