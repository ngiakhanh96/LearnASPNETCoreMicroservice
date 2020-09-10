using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _context.Redis.StringGetAsync(userName);

            return basket.IsNullOrEmpty 
                ? null 
                : JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated = await _context.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return !updated 
                ? null 
                : await GetBasket(basket.UserName);
        }

        public Task<bool> DeleteBasket(string userName)
        {
            return _context.Redis.KeyDeleteAsync(userName);
        }
    }
}
