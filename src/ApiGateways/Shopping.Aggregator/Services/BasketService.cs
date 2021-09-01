using Shopping.Aggregator.Models;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        public BasketService()
        {

        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
