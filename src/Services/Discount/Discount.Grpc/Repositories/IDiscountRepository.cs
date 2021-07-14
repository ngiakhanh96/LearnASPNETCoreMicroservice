using System;
using System.Threading.Tasks;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetCoupon(Guid id);

        Task<Coupon> GetCouponByProductName(string productName);

        Task<Guid> CreateCoupon(Coupon coupon);

        Task<bool> UpdateCoupon(Coupon coupon);

        Task<bool> DeleteCoupon(Guid id);
    }
}
