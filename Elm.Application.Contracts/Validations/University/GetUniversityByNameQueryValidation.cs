using Elm.Application.Contracts.Features.University.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.University
{
    public sealed class GetUniversityByNameQueryValidation : AbstractValidator<GetUniversityByNameQuery>
    {
        public GetUniversityByNameQueryValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("University name must not be empty.");
        }
    }
}