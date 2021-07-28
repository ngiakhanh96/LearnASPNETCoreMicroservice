using FluentValidation;
using Shared.Validator;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : BaseValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(cmd => cmd.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required.")
                .MaximumLength(50)
                .WithMessage("{UserName} must not exceed 50 characters");

            RuleFor(cmd => cmd.EmailAddress)
                .NotEmpty();

            RuleFor(cmd => cmd.TotalPrice)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
