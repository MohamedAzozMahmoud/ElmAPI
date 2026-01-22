using Elm.Application.Contracts.Features.College.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.College
{
    public sealed class DeleteCollegeCommandValidation : AbstractValidator<DeleteCollegeCommand>
    {
        public DeleteCollegeCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("College Id must be a positive integer.");
        }
    }
}
