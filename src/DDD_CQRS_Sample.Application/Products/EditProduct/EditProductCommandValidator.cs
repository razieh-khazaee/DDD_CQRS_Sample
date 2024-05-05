using FluentValidation;

namespace DDD_CQRS_Sample.Application.Products.EditProduct;

internal class EditProductCommandValidator : AbstractValidator<EditProductCommand>
{
    public EditProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
             .NotEmpty();

        RuleFor(c => c.Brand)
            .NotEmpty();

        RuleFor(m => m.Price)
           .NotEmpty();

        RuleFor(m => m.ImageId)
            .GreaterThan(0)
            .When(m => m != null);
    }
}
