using Elm.Application.Contracts.Features.University.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.University
{
    public sealed class UpdateUniversityCommandValidation : AbstractValidator<UpdateUniversityCommand>
    {
        public UpdateUniversityCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("University Id must be greater than zero.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("University name is required.")
                .MaximumLength(50).WithMessage("University name must not exceed 50 characters.");
        }
    }
}