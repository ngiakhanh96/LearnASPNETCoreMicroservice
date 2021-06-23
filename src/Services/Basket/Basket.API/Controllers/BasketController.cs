using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet(Name = nameof(GetBasket))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Entities.Basket>> GetBasket(string userName)
        {
            return await _basketRepository.GetBasket(userName) ?? new Entities.Basket(userName, new List<BasketItem>());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Entities.Basket>> UpsertBasket([FromBody] Entities.Basket shoppingCart)
        {
            var basket = await _basketRepository.UpsertBasket(shoppingCart);
            return CreatedAtAction(nameof(GetBasket), new { userName = basket.UserName}, basket);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}
