using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class DeleteQuestionCommandValidation : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidation()
        {
            RuleFor(x => x.questionId)
                .GreaterThan(0).WithMessage("Question Id must be greater than zero.");
        }
    }
}
