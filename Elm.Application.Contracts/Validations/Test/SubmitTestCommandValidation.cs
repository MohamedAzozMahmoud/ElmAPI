using Elm.Application.Contracts.Features.Test.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Test
{
    public sealed class SubmitTestCommandValidation : AbstractValidator<SubmitTestCommand>
    {
        public SubmitTestCommandValidation()
        {
            RuleFor(x => x.TestSessionId)
                .NotEmpty()
                .WithMessage("TestSessionId must not be empty.");
            RuleFor(x => x.Answers)
                .NotNull()
                .WithMessage("Answers must not be null.")
                .Must(answers => answers.Count > 0)
                .WithMessage("Answers must contain at least one answer.");
        }
    }
}
