using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.DTO;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet(Name = nameof(GetBasket))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDTO>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName) ?? new Entities.Basket(userName, new List<BasketItem>());
            return _mapper.Map<BasketDTO>(basket);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDTO>> UpsertBasket([FromBody] BasketDTO basketDto)
        {
            var basket = await _basketRepository.UpsertBasket(_mapper.Map<Entities.Basket>(basketDto));
            foreach (var basketItem in basket.Items)
            {
                try
                {
                    var coupon = await _discountGrpcService.GetDiscount(basketItem.ProductName);
                    basketItem.Price -= coupon.Amount;
                }
                catch (RpcException exception) when (exception.StatusCode == Grpc.Core.StatusCode.NotFound)
                {
                }
            }
            return CreatedAtAction(nameof(GetBasket), new { userName = basket.UserName}, _mapper.Map<BasketDTO>(basket));
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
