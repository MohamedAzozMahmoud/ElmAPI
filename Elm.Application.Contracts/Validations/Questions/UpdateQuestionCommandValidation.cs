using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class UpdateQuestionCommandValidation : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Question Id must be greater than zero.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Question content must not be empty.")
                .MaximumLength(1000).WithMessage("Question content must not exceed 1000 characters.");
            RuleFor(x => x.QuestionType)
                .NotEmpty().WithMessage("Question type must not be empty.")
                .MaximumLength(100).WithMessage("Question type must not exceed 100 characters.");
        }
    }
}
