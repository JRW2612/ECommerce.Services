using Basket.Service.Entities;
using Basket.Service.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Service.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }
        public async Task DeleteBasketAsync(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            var result = JsonConvert.DeserializeObject<ShoppingCart>(basket);
            return result;
        }
        public async Task<ShoppingCart> UpsertBasketAsync(ShoppingCart shoppingCart)
        {
            var cart = JsonConvert.SerializeObject(shoppingCart);
            await _redisCache.SetStringAsync(shoppingCart.UserName, cart);

            var getUser = await GetBasketAsync(shoppingCart.UserName);
            return getUser;
        }
    }
}
