using Elm.Application.Contracts.Features.University.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.University
{
    public sealed class GetUniversityByIdQueryValidation : AbstractValidator<GetUniversityByIdQuery>
    {
        public GetUniversityByIdQueryValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("University Id must be greater than zero.");
        }
    }
}