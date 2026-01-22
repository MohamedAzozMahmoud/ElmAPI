using Elm.Application.Contracts.Features.Questions.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddQuestionsDtoValidation : AbstractValidator<AddQuestionsDto>
    {
        public AddQuestionsDtoValidation()
        {
            RuleFor(x => x.Content)
                 .NotEmpty().WithMessage("Question content must not be empty.")
                 .MaximumLength(1000).WithMessage("Question content must not exceed 1000 characters.");
            RuleFor(x => x.QuestionType)
                .NotEmpty().WithMessage("Question type must not be empty.")
                .Must(type => new[] { "MultipleChoice", "TrueFalse", "ShortAnswer" }.Contains(type))
                .WithMessage("Question type must be one of the following: MultipleChoice, TrueFalse, ShortAnswer.");
        }
    }
}
