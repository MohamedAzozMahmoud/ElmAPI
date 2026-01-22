using Elm.Application.Contracts.Features.Department.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Department
{
    public sealed class UpdateDepartmentCommandValidation : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Department ID must be a positive integer.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");
        }
    }
}
