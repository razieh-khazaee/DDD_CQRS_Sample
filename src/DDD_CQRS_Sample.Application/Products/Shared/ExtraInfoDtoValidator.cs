using FluentValidation;

namespace DDD_CQRS_Sample.Application.Products.Shared
{
    public class ExtraInfoDtoValidator : AbstractValidator<ExtraInfoDto>
    {
        public ExtraInfoDtoValidator()
        {
            RuleFor(m => m.Key)
                .NotEmpty();

            RuleFor(m => m.Value)
               .NotEmpty();
        }
    }
}
