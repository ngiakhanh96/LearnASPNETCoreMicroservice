﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.API.DTO;
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
        private readonly IMapper _mapper;

        public DiscountController(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = nameof(GetCoupon))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CouponDTO>> GetCoupon(Guid id)
        {
            var coupon = await _discountRepository.GetCoupon(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return _mapper.Map<CouponDTO>(coupon);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CouponDTO>> CreateCoupon([FromBody] CouponDTO couponDto)
        {
            var insertedId = await _discountRepository.CreateCoupon(_mapper.Map<Coupon>(couponDto));
            var newCoupon = await _discountRepository.GetCoupon(insertedId);
            return CreatedAtRoute(nameof(GetCoupon), new {id = newCoupon.Id}, _mapper.Map<CouponDTO>(newCoupon));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateCoupon([FromBody] CouponDTO couponDto)
        {
            return await _discountRepository.UpdateCoupon(_mapper.Map<Coupon>(couponDto));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteCoupon(Guid id)
        {
            return await _discountRepository.DeleteCoupon(id);
        }
    }
}
