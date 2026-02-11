using Elm.Application.Contracts.Features.Questions.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddQuestionsDtoValidation : AbstractValidator<AddQuestionsDto>
    {
        public AddQuestionsDtoValidation()
        {
            RuleFor(x => x.Content)
                 .NotEmpty().WithMessage("محتوى السؤال مطلوب.")
                 .MaximumLength(500).WithMessage("محتوى السؤال لا يجب أن يتجاوز 500 حرف.");
            RuleFor(x => x.QuestionType)
                .NotEmpty().WithMessage("نوع السؤال مطلوب.")
                .Must(type => new[] { "MCQ", "TrueFalse" }.Contains(type))
                .WithMessage("نوع السؤال يجب أن يكون إما MCQ أو TrueFalse.");
        }
    }
}
