using FluentValidation;

namespace Shared.Validator
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected BaseValidator()
        {
            RuleFor(t => t).NotNull();
        }
    }
}
