using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<Entities.Basket> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            return string.IsNullOrWhiteSpace(basket) 
                ? null 
                : JsonSerializer.Deserialize<Entities.Basket>(basket);
        }

        public async Task<Entities.Basket> UpsertBasket(Entities.Basket shoppingCart)
        {
            await _redisCache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart));
            return await GetBasket(shoppingCart.UserName);
        }

        public Task DeleteBasket(string userName)
        {
            return _redisCache.RemoveAsync(userName);
        }
    }
}
