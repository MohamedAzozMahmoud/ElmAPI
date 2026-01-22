using Elm.Application.Contracts.Features.Year.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Year
{
    public sealed class GetAllYearQueryValidation : AbstractValidator<GetAllYearQuery>
    {
        public GetAllYearQueryValidation()
        {
            RuleFor(x => x.collegeId)
                .GreaterThan(0).WithMessage("College ID must be a positive integer.");
        }
    }
}
