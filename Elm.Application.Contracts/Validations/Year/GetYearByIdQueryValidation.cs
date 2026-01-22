using Elm.Application.Contracts.Features.Year.Queries;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Year
{
    public sealed class GetYearByIdQueryValidation : AbstractValidator<GetYearByIdQuery>
    {
        public GetYearByIdQueryValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Year ID must be a positive integer.");
        }
    }
}
