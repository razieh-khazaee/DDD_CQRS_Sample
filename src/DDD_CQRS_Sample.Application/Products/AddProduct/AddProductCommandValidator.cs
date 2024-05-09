using DDD_CQRS_Sample.Application.Products.Shared;
using FluentValidation;

namespace DDD_CQRS_Sample.Application.Products.AddProduct;

internal class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();

        RuleFor(c => c.Brand)
            .NotEmpty();

        RuleFor(m => m.Price)
            .NotEmpty();

        RuleFor(m => m.ImageId)
           .GreaterThan(0)
           .When(m => m != null);

        RuleForEach(m => m.ExtraInfos)
            .SetValidator(new ExtraInfoDtoValidator());
    }
}