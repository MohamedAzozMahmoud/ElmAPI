using Elm.Application.Contracts.Features.Options.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddOptionsDtoValidation : AbstractValidator<AddOptionsDto>
    {
        public AddOptionsDtoValidation()
        {
            RuleFor(x => x.Content)
                  .NotEmpty().WithMessage("محتوى الخيار مطلوب.")
                  .MaximumLength(500).WithMessage("محتوى الخيار لا يجب أن يتجاوز 500 حرف.");
            RuleFor(x => x.IsCorrect)
                .NotNull().WithMessage("يجب تحديد صحة الخيار.");
        }
    }
}
