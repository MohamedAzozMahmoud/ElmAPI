using Elm.Application.Contracts.Features.Department.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Department
{
    public sealed class AddDepartmentCommandValidation : AbstractValidator<AddDepartmentCommand>
    {
        public AddDepartmentCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");
            RuleFor(x => x.collegeId)
                .GreaterThan(0).WithMessage("College ID must be a positive integer.");
        }
    }
}
