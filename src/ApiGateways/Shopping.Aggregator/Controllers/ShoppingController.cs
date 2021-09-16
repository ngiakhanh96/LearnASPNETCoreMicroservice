using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ILogger<ShoppingController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ILogger<ShoppingController> logger, ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet(Name = nameof(GetShopping))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);

            var requests = new List<Task>();
            foreach (var basketItemExtendedModel in basket.Items)
            {
                async Task UpdateBasketItemInfo()
                {
                    var product = await _catalogService.GetCatalog(basketItemExtendedModel.ProductId);
                    basketItemExtendedModel.ProductName = product.Name;
                    basketItemExtendedModel.Category = product.Category;
                    basketItemExtendedModel.Summary = product.Summary;
                    basketItemExtendedModel.Description = product.Description;
                    basketItemExtendedModel.ImageFile = product.ImageFile;
                }
                requests.Add(UpdateBasketItemInfo());
            }

            var orders = await _orderService.GetOrdersByUserName(userName);
            await Task.WhenAll(requests);

            return new ShoppingModel
            {
                UserName = userName,
                BasketWithProduct = basket,
                Orders = orders
            };
        }

    }
}
