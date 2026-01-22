using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddRingQuestionsCommandValidation : AbstractValidator<AddRingQuestionsCommand>
    {
        public AddRingQuestionsCommandValidation()
        {
            RuleFor(x => x.questionsBankId)
                .GreaterThan(0).WithMessage("Question Bank Id must be greater than zero.");
            RuleForEach(x => x.QuestionsDtos).SetValidator(new AddQuestionsDtoValidation());
            RuleForEach(x => x.QuestionsDtos.SelectMany(q => q.Options)).SetValidator(new AddOptionsDtoValidation());
        }
    }
}
