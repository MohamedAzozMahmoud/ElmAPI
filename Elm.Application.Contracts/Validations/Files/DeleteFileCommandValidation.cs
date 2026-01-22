using Elm.Application.Contracts.Features.Files.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Files
{
    public sealed class DeleteFileCommandValidation : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("File ID must be a positive integer.");
        }
    }
}
