using Orders.Service.Entities;

namespace Orders.Service.Repositories.Interfaces
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersByUserNameAsync(string userName);
        Task AddOutboxMessageAsync(OutBoxMessage outBoxMessage);
    }
}
