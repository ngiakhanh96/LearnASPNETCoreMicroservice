using Discount.API.Entities;
using FluentValidation;

namespace Discount.API.Validations
{
    public class CouponValidator : AbstractValidator<Coupon>
    {
        public CouponValidator()
        { 
            RuleFor(coupon => coupon.ProductName).NotEmpty().MaximumLength(24);
            RuleFor(coupon => coupon.Description).NotEmpty();
            RuleFor(coupon => coupon.Id).NotEmpty();
            RuleFor(coupon => coupon.Amount).NotEmpty();
        }
    }
}
