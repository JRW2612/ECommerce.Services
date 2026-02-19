using Basket.Service.Entities;

namespace Basket.Service.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName);
        Task<ShoppingCart> UpsertBasketAsync(ShoppingCart shoppingCart);
        Task DeleteBasketAsync(string userName);
    }
}
