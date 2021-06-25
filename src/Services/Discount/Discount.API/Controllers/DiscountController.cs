using System;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{id:guid}", Name = nameof(GetCoupon))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetCoupon(Guid id)
        {
            return await _discountRepository.GetCoupon(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] Coupon discount)
        {
            var insertedId = await _discountRepository.CreateCoupon(discount);
            var newCoupon = await _discountRepository.GetCoupon(insertedId);
            return CreatedAtRoute(nameof(GetCoupon), new {id = newCoupon.Id}, newCoupon);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateCoupon([FromBody] Coupon discount)
        {
            return await _discountRepository.UpdateCoupon(discount);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteCoupon(Guid id)
        {
            return await _discountRepository.DeleteCoupon(id);
        }
    }
}
