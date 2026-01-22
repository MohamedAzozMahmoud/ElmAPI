using Elm.Application.Contracts.Features.Year.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Year
{
    public sealed class DeleteYearCommandValidation : AbstractValidator<DeleteYearCommand>
    {
        public DeleteYearCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Year ID must be a positive integer.");
        }
    }
}
