using Elm.Application.Contracts.Features.University.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.University
{
    public sealed class AddUniversityCommandValidation : AbstractValidator<AddUniversityCommand>
    {
        public AddUniversityCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("University name is required.")
                .MaximumLength(50).WithMessage("University name must not exceed 50 characters.");
        }
    }
}