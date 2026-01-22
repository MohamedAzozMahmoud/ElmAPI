using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddQuestionCommandValidation : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidation()
        {
            RuleFor(x => x.questionBankId)
                .GreaterThan(0).WithMessage("Question Bank Id must be greater than zero.");
            RuleFor(x => x.QuestionsDto)
                .NotNull().WithMessage("Questions data must be provided.");
            RuleForEach(x => x.QuestionsDto.Options).SetValidator(new AddOptionsDtoValidation());
            RuleFor(x => x.QuestionsDto).SetValidator(new AddQuestionsDtoValidation());
        }
    }
}
