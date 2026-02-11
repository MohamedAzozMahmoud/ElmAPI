using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddQuestionCommandValidation : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidation()
        {
            RuleFor(x => x.questionBankId)
                .GreaterThan(0).WithMessage("معرف بنك الأسئلة يجب أن يكون أكبر من صفر.");
            RuleFor(x => x.QuestionsDto)
                .NotNull().WithMessage("بيانات الأسئلة مطلوبة.");
            RuleForEach(x => x.QuestionsDto.Options).SetValidator(new AddOptionsDtoValidation());
            RuleFor(x => x.QuestionsDto).SetValidator(new AddQuestionsDtoValidation());
        }
    }
}
