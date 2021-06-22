using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> GetBasket(string userName);

        Task<Entities.Basket> UpsertBasket(Entities.Basket shoppingCart);

        Task DeleteBasket(string userName);
    }
}
