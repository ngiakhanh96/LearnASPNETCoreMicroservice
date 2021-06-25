using Basket.API.DTO;
using FluentValidation;
using Shared.Validator;

namespace Basket.API.Validators
{
    public class BasketDtoValidator : BaseValidator<BasketDTO>
    {
        public BasketDtoValidator()
        {
            RuleFor(basket => basket.Items).NotNull();
            RuleFor(basket => basket.UserName).NotEmpty();
        }
    }
}
