using Elm.Application.Contracts.Features.Subject.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Subject
{
    public sealed class AddSubjectCommandValidation : AbstractValidator<AddSubjectCommand>
    {
        public AddSubjectCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(100).WithMessage("Subject name must not exceed 100 characters.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Subject code is required.")
                .MaximumLength(20).WithMessage("Subject code must not exceed 20 characters.");
        }
    }
}
