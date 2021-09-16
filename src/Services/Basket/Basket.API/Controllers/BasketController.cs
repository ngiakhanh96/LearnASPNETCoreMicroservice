using AutoMapper;
using Basket.API.DTO;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet(Name = nameof(GetBasket))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDTO>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName) ?? new Entities.Basket(userName, new List<BasketItem>());
            return _mapper.Map<BasketDTO>(basket);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BasketDTO>> UpsertBasket([FromBody] BasketDTO basketDto)
        {
            var discountUpdateTasks = new List<Task>();
            foreach (var basketItem in basketDto.Items)
            {
                async Task UpdateDiscount()
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
                discountUpdateTasks.Add(UpdateDiscount());
            }

            await Task.WhenAll(discountUpdateTasks);
            var basket = await _basketRepository.UpsertBasket(_mapper.Map<Entities.Basket>(basketDto));
            return CreatedAtAction(nameof(GetBasket), new { userName = basket.UserName }, _mapper.Map<BasketDTO>(basket));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutDTO basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            await _basketRepository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }
    }
}
