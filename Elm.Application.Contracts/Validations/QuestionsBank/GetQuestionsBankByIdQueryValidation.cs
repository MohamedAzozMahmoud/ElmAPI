using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.QuestionsBank
{
    public sealed class GetQuestionsBankByIdQueryValidation : AbstractValidator<GetQuestionsBankByIdQuery>
    {
        public GetQuestionsBankByIdQueryValidation()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Questions bank ID must be a positive integer.");
        }
    }
}
