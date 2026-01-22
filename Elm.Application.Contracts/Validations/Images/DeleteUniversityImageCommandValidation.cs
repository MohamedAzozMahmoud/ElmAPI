using Elm.Application.Contracts.Features.Images.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Images
{
    public sealed class DeleteUniversityImageCommandValidation : AbstractValidator<DeleteUniversityImageCommand>
    {
        public DeleteUniversityImageCommandValidation()
        {
            RuleFor(x => x.universityId)
                .GreaterThan(0).WithMessage("University ID must be a positive integer.");
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Image ID must be a positive integer.");
        }
    }
}
