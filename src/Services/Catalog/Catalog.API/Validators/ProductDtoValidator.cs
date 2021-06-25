using Catalog.API.DTO;
using FluentValidation;
using Shared.Validator;

namespace Catalog.API.Validators
{
    public class ProductDtoValidator : BaseValidator<ProductDTO>
    {
        public ProductDtoValidator()
        {
            RuleFor(prod => prod.Name).NotEmpty().MaximumLength(24);
            RuleFor(prod => prod.Category).NotEmpty();
            RuleFor(prod => prod.Description).NotNull();
            RuleFor(prod => prod.Id).MaximumLength(24).When(prod => !string.IsNullOrWhiteSpace(prod.Id));
            RuleFor(prod => prod.Price).Must(price => price > 0);
            RuleFor(prod => prod.ImageFile).NotNull();
            RuleFor(prod => prod.Summary).NotNull();
        }
    }
}
