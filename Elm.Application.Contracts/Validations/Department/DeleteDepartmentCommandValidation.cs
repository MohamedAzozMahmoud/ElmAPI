using Elm.Application.Contracts.Features.Department.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Department
{
    public sealed class DeleteDepartmentCommandValidation : AbstractValidator<DeleteDepartmentCommand>
    {
        public DeleteDepartmentCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Department ID must be a positive integer.");
        }
    }
}
