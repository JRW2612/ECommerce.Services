using MediatR;
using Orders.Service.DTOs;
using Orders.Service.Mapper;
using Orders.Service.Queries;
using Orders.Service.Repositories.Interfaces;


namespace Orders.Service.Handlers
{
    public class GetOrderListHandler : IRequestHandler<GetOrderListQuery, List<OrderDTO>>
    {
        private readonly IOrderRepository _oRepository;

        public GetOrderListHandler(IOrderRepository oRepository)
        {
            _oRepository = oRepository;
        }

        public async Task<List<OrderDTO>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orders = await _oRepository.GetAllOrdersByUserNameAsync(request.UserName);

            return orders.Select(s => s.ToDto()).ToList();
        }
    }
}
