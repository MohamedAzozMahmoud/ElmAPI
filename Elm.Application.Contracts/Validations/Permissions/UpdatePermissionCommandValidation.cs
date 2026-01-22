using Elm.Application.Contracts.Features.Permissions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Permissions
{
    public sealed class UpdatePermissionCommandValidation : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Permission ID must be greater than zero.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
        }
    }
}
