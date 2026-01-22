using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.QuestionsBank
{
    public sealed class GetAllQuestionsBankQueryValidation : AbstractValidator<GetAllQuestionsBankQuery>
    {
        public GetAllQuestionsBankQueryValidation()
        {
            RuleFor(x => x.curriculumId)
                .GreaterThan(0).WithMessage("Curriculum ID must be a positive integer.");
        }
    }
}
