using Elm.Application.Contracts.Features.College.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.College
{
    public sealed class AddCollegeCommandValidation : AbstractValidator<AddCollegeCommand>
    {
        public AddCollegeCommandValidation()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("College name is required.")
                .MaximumLength(100).WithMessage("College name must not exceed 100 characters.");
            RuleFor(x => x.UniversityId)
                .GreaterThan(0).WithMessage("UniversityId must be a positive integer.");
        }
    }
}
