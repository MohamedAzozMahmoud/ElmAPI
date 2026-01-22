using Elm.Application.Contracts.Features.Permissions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Permissions
{
    public sealed class DeletePermissionCommandValidation : AbstractValidator<DeletePermissionCommand>
    {
        public DeletePermissionCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Permission ID must be greater than zero.");
        }
    }
}
