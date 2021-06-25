using System.Collections.Generic;

namespace Basket.API.DTO
{
    public class BasketDTO
    {
        public string UserName { get; init; }

        public List<BasketItemDTO> Items { get; set; }

        public decimal TotalPrice { get; set; }

        public BasketDTO(string userName, List<BasketItemDTO> items, decimal totalPrice)
        {
            UserName = userName;
            Items = items;
            TotalPrice = totalPrice;
        }
    }
}
