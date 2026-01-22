using Elm.Application.Contracts.Features.University.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.University
{
    public sealed class DeleteUniversityCommandValidation : AbstractValidator<DeleteUniversityCommand>
    {
        public DeleteUniversityCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("University Id must be greater than zero.");
        }
    }
}