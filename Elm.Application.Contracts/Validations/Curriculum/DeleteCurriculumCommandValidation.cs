using Elm.Application.Contracts.Features.Curriculum.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Curriculum
{
    public sealed class DeleteCurriculumCommandValidation : AbstractValidator<DeleteCurriculumCommand>
    {
        public DeleteCurriculumCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
