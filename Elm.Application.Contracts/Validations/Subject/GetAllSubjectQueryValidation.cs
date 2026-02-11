using Elm.Application.Contracts.Features.Subject.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Subject
{
    public sealed class GetAllSubjectQueryValidation : AbstractValidator<GetAllSubjectByDepartmentIdQuery>
    {
        public GetAllSubjectQueryValidation()
        {
            RuleFor(x => x.departmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than zero.");
        }
    }
}
