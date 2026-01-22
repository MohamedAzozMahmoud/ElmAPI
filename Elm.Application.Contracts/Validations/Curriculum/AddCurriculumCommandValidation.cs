using Elm.Application.Contracts.Features.Curriculum.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Curriculum
{
    public sealed class AddCurriculumCommandValidation : AbstractValidator<AddCurriculumCommand>
    {
        public AddCurriculumCommandValidation()
        {
            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("SubjectId must be greater than 0.");
            RuleFor(x => x.YearId)
                .GreaterThan(0).WithMessage("YearId must be greater than 0.");
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be greater than 0.");
        }
    }
}
