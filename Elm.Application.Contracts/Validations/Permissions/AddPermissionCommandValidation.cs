using Elm.Application.Contracts.Features.Permissions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Permissions
{
    public sealed class AddPermissionCommandValidation : AbstractValidator<AddPermissionCommand>
    {
        public AddPermissionCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
        }
    }
}
