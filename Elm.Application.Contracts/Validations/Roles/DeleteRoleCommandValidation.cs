using Elm.Application.Contracts.Features.Roles.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Roles
{
    public class DeleteRoleCommandValidation : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(25).WithMessage("Role name must not exceed 25 characters.");
        }
    }
}
