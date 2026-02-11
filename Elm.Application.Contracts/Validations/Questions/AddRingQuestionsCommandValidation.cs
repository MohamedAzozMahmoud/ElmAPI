using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddRingQuestionsCommandValidation : AbstractValidator<AddRingQuestionsCommand>
    {
        public AddRingQuestionsCommandValidation()
        {
            RuleFor(x => x.questionsBankId)
                .GreaterThan(0).WithMessage("معرف بنك الأسئلة يجب أن يكون أكبر من صفر.");
            RuleForEach(x => x.QuestionsDtos).SetValidator(new AddQuestionsDtoValidation());
            RuleForEach(x => x.QuestionsDtos.SelectMany(q => q.Options)).SetValidator(new AddOptionsDtoValidation());
        }
    }
}
