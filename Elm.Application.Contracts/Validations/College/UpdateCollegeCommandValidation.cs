using Elm.Application.Contracts.Features.College.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.College
{
    public sealed class UpdateCollegeCommandValidation : AbstractValidator<UpdateCollegeCommand>
    {
        public UpdateCollegeCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("College Id must be a positive integer.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("College name is required.")
                .MaximumLength(100).WithMessage("College name must not exceed 100 characters.");
        }
    }
}
