using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class Basket
    {
        public string UserName { get; init; }

        public List<BasketItem> Items { get; set; }

        public Basket(string userName, List<BasketItem> items)
        {
            UserName = userName;
            Items = items;
        }

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(shoppingCartItem => shoppingCartItem.Price * shoppingCartItem.Quantity);
            }
        }
    }
}
