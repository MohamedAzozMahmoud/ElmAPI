using Elm.Application.Contracts.Features.Subject.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Subject
{
    public sealed class UpdateSubjectCommandValidation : AbstractValidator<UpdateSubjectCommand>
    {
        public UpdateSubjectCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Subject Id must be greater than zero.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(100).WithMessage("Subject name must not exceed 100 characters.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Subject code is required.")
                .MaximumLength(20).WithMessage("Subject code must not exceed 20 characters.");
        }
    }
}
