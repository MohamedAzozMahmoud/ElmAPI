using Elm.Application.Contracts.Features.Options.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddOptionsDtoValidation : AbstractValidator<AddOptionsDto>
    {
        public AddOptionsDtoValidation()
        {
            RuleFor(x => x.Content)
                  .NotEmpty().WithMessage("Option content must not be empty.")
                  .MaximumLength(500).WithMessage("Option content must not exceed 500 characters.");
            RuleFor(x => x.IsCorrect)
                .NotNull().WithMessage("Option correctness must be specified.");
        }
    }
}
