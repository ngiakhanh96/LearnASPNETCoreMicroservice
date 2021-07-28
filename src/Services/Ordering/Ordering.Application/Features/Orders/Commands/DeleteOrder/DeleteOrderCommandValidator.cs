using FluentValidation;
using Shared.Validator;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandValidator : BaseValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
        }
    }
}
