using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.QuestionsBank
{
    public sealed class DeleteQuestionsBankCommandValidation : AbstractValidator<DeleteQuestionsBankCommand>
    {
        public DeleteQuestionsBankCommandValidation()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Questions bank ID must be a positive integer.");
        }
    }
}
