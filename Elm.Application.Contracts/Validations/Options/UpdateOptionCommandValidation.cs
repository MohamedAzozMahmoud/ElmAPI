using Elm.Application.Contracts.Features.Options.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Options
{
    public sealed class UpdateOptionCommandValidation : AbstractValidator<UpdateOptionCommand>
    {
        public UpdateOptionCommandValidation()
        {
            RuleFor(x => x.optionId)
                .GreaterThan(0).WithMessage("Option ID must be a positive integer.");
            RuleFor(x => x.content)
                .NotEmpty().WithMessage("Option content must not be empty.")
                .MaximumLength(500).WithMessage("Option content must not exceed 500 characters.");
            RuleFor(x => x.isCorrect)
                .NotNull().WithMessage("IsCorrect must be specified.");
        }
    }
}
