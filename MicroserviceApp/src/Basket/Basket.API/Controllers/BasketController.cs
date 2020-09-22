using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _logger;
        private readonly EventBusRabbitMQProducer _eventBusRabbitMqProducer;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketRepository basketRepository,
            ILogger<BasketController> logger,
            EventBusRabbitMQProducer eventBusRabbitMqProducer, 
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _logger = logger;
            _eventBusRabbitMqProducer = eventBusRabbitMqProducer;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            if (basket != null)
                return basket;
            _logger.LogWarning($"Basket with username: {userName} not found");
            return new BasketCart(userName);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basketCart)
        {
            return await _basketRepository.UpdateBasket(basketCart);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> DeleteBasket([StringLength(24), FromRoute] string id)
        {
            return await _basketRepository.DeleteBasket(id);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await _basketRepository.DeleteBasket(basket.UserName);
            if (!basketRemoved)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            _eventBusRabbitMqProducer.PublishBasketCheckout(
                EventBusConstants.BasketCheckoutQueue, 
                eventMessage);

            return Accepted();
        }
    }
}
