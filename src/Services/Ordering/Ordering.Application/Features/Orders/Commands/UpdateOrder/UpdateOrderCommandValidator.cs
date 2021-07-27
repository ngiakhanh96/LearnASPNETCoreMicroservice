using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Shared.Validator;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : BaseValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(cmd => cmd.Id)
                .NotEmpty();

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
