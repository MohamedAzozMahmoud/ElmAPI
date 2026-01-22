using Elm.Application.Contracts.Features.Subject.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Subject
{
    public sealed class DeleteSubjectCommandValidation : AbstractValidator<DeleteSubjectCommand>
    {
        public DeleteSubjectCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Subject Id must be greater than zero.");
        }
    }
}
