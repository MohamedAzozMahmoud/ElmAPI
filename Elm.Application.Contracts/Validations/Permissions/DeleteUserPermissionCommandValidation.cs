using Elm.Application.Contracts.Features.Permissions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Permissions
{
    public sealed class DeleteUserPermissionCommandValidation : AbstractValidator<DeleteUserPermissionCommand>
    {
        public DeleteUserPermissionCommandValidation()
        {
            RuleFor(x => x.userName)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(100).WithMessage("User name must not exceed 100 characters.");
            RuleFor(x => x.permissionName)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
        }
    }
}
