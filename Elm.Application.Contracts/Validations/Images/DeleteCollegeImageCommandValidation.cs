using Elm.Application.Contracts.Features.Images.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Images
{
    public sealed class DeleteCollegeImageCommandValidation : AbstractValidator<DeleteCollegeImageCommand>
    {
        public DeleteCollegeImageCommandValidation()
        {
            RuleFor(x => x.collegeId)
                .GreaterThan(0).WithMessage("College ID must be a positive integer.");
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Image ID must be a positive integer.");
        }
    }
}
