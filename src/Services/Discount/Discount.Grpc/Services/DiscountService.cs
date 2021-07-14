using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponDTO> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetCouponByProductName(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }
            return _mapper.Map<CouponDTO>(coupon);
        }

        public override async Task<CouponDTO> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var insertedId = await _discountRepository.CreateCoupon(_mapper.Map<Coupon>(request.Coupon));
            var newCoupon = await _discountRepository.GetCoupon(insertedId);
            if (newCoupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }
            return _mapper.Map<CouponDTO>(newCoupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return new()
            {
                Success = await _discountRepository.DeleteCoupon(Guid.Parse(request.Id))
            };
        }

        public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return new()
            {
                Success = await _discountRepository.UpdateCoupon(_mapper.Map<Coupon>(request.Coupon))
            };
        }
    }
}
