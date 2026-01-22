using Elm.Application.Contracts.Features.Images.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Images
{
    public sealed class AddUniversityImageCommandValidation : AbstractValidator<AddUniversityImageCommand>
    {
        public AddUniversityImageCommandValidation()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Image file must be provided.")
                .Must(file => file != null && (file.ContentType == "image/jpeg" || file.ContentType == "image/png"
                      || file.ContentType == "image/gif" || file.ContentType == "image/bmp" || file.ContentType == "image/svg+xml"))
                .WithMessage("Only JPEG, PNG, GIF, BMP, and SVG image formats are allowed.")
                .Must(file => file != null && file.Length <= 5 * 1024 * 1024) // 5 MB limit
                .WithMessage("Image size must not exceed 5 MB.");
        }
    }
}
