using MediatR;
using Orders.Service.DTOs;


namespace Orders.Service.Queries
{
    public record GetOrderListQuery(string? UserName) : IRequest<List<OrderDTO>>;
}
