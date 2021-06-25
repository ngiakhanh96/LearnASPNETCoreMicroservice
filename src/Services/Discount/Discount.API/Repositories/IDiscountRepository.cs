using System;
using System.Threading.Tasks;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetCoupon(Guid id);

        Task<Guid> CreateCoupon(Coupon coupon);

        Task<bool> UpdateCoupon(Coupon coupon);

        Task<bool> DeleteCoupon(Guid id);
    }
}
