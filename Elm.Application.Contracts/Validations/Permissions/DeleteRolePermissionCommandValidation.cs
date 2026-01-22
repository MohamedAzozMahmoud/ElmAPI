using Elm.Application.Contracts.Features.Permissions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Permissions
{
    public sealed class DeleteRolePermissionCommandValidation : AbstractValidator<DeleteRolePermissionCommand>
    {
        public DeleteRolePermissionCommandValidation()
        {
            RuleFor(x => x.roleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
            RuleFor(x => x.permissionName)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
        }
    }
}
