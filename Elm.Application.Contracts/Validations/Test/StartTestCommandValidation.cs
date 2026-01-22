using Elm.Application.Contracts.Features.Test.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Test
{
    public sealed class StartTestCommandValidation : AbstractValidator<StartTestCommand>
    {
        public StartTestCommandValidation()
        {
            RuleFor(x => x.QuestionsBankId)
                .GreaterThan(0)
                .WithMessage("QuestionsBankId must be greater than 0.");
            RuleFor(x => x.NumberOfQuestions)
                .GreaterThan(0)
                .WithMessage("NumberOfQuestions must be greater than 0.");
        }
    }
}
