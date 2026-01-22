using Elm.Application.Contracts.Features.Year.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Year
{
    public sealed class UpdateYearCommandValidation : AbstractValidator<UpdateYearCommand>
    {
        public UpdateYearCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Year ID must be a positive integer.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Year name is required.")
                .MaximumLength(100).WithMessage("Year name must not exceed 100 characters.");
        }
    }
}
