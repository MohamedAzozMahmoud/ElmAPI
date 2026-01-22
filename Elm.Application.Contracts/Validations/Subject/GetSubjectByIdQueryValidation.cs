using Elm.Application.Contracts.Features.Subject.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Subject
{
    public sealed class GetSubjectByIdQueryValidation : AbstractValidator<GetSubjectByIdQuery>
    {
        public GetSubjectByIdQueryValidation()
        {
            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("Subject ID must be greater than zero.");
        }
    }
}
