using Elm.Application.Contracts.Features.Roles.DTOs;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Roles
{
    public class RoleDtoValidation : AbstractValidator<RoleDto>
    {
        public RoleDtoValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(256).WithMessage("Role name must not exceed 256 characters.");
        }
    }
}
