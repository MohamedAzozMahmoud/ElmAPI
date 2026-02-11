using Elm.Application.Contracts.Features.Files.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Files
{
    public sealed class RatingFileCommandValidation : AbstractValidator<RatingFileCommand>
    {
        public RatingFileCommandValidation()
        {
            RuleFor(x => x.fileId)
                .GreaterThan(0).WithMessage("File ID must be a positive integer.");
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("User ID cannot be empty.");
            RuleFor(x => x.comment)
                .NotEmpty().WithMessage("Comment cannot be empty.")
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
            RuleFor(x => x.rating)
                .IsInEnum().WithMessage("Invalid rating value.");
        }
    }
}
