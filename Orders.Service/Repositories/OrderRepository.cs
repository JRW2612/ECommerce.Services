using Microsoft.EntityFrameworkCore;
using Orders.Service.Data;
using Orders.Service.Entities;
using Orders.Service.Repositories.Interfaces;

namespace Orders.Service.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context) : base(context) { }
        public async Task AddOutboxMessageAsync(OutBoxMessage outBoxMessage)
        {
            await _context.OutBoxMessages.AddAsync(outBoxMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByUserNameAsync(string userName)
        {
            var orderLists = await _context.Orders.AsNoTracking()
                .Where(o => o.UserName == userName)
                .ToListAsync();
            return orderLists;
        }


    }
}
