using Elm.Application.Contracts.Features.Roles.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Roles
{
    public class UpdateRoleCommandValidation : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidation()
        {
            RuleFor(x => x.oldName)
                .NotEmpty().WithMessage("Old role name is required.")
                .MaximumLength(30).WithMessage("Old role name must not exceed 30 characters.");
            RuleFor(x => x.newName)
                .NotEmpty().WithMessage("New role name is required.")
                .MaximumLength(30).WithMessage("New role name must not exceed 30 characters.");
        }
    }
}
