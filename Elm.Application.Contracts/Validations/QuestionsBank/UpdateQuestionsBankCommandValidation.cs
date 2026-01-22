using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.QuestionsBank
{
    public sealed class UpdateQuestionsBankCommandValidation : AbstractValidator<UpdateQuestionsBankCommand>
    {
        public UpdateQuestionsBankCommandValidation()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Questions bank ID must be a positive integer.");
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Questions bank name is required.")
                .MaximumLength(200).WithMessage("Questions bank name must not exceed 200 characters.");
            RuleFor(x => x.curriculumId)
                .GreaterThan(0).WithMessage("Curriculum ID must be a positive integer.");
        }
    }
}
