using Elm.Application.Contracts.Features.Options.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Options
{
    public sealed class DeleteOptionCommandValidation : AbstractValidator<DeleteOptionCommand>
    {
        public DeleteOptionCommandValidation()
        {
            RuleFor(x => x.optionId)
                .GreaterThan(0).WithMessage("Option ID must be a positive integer.");
        }
    }
}
