using Discount.API.DTO;
using FluentValidation;
using Shared.Validator;

namespace Discount.API.Validators
{
    public class CouponDtoValidator : BaseValidator<CouponDTO>
    {
        public CouponDtoValidator()
        {
            RuleFor(coupon => coupon.ProductName).NotEmpty().MaximumLength(24);
            RuleFor(coupon => coupon.Description).NotNull();
            RuleFor(coupon => coupon.Id).NotEmpty();
        }
    }
}
