using Elm.Application.Contracts.Features.Year.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Year
{
    public sealed class AddYearCommandValidation : AbstractValidator<AddYearCommand>
    {
        public AddYearCommandValidation()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Year name is required.")
                .MaximumLength(100).WithMessage("Year name must not exceed 100 characters.");
            RuleFor(x => x.collegeId)
                .GreaterThan(0).WithMessage("College ID must be a positive integer.");
        }
    }
}
