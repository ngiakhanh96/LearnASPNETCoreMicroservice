using StackExchange.Redis;

namespace Basket.API.Data.Interfaces
{
    public interface IBasketContext
    {
        public IDatabase Redis { get; }
    }
}
