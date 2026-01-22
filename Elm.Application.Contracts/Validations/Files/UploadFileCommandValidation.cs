using Elm.Application.Contracts.Features.Files.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Files
{
    public sealed class UploadFileCommandValidation : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidation()
        {
            RuleFor(x => x.curriculumId)
                .GreaterThan(0).WithMessage("Curriculum ID must be a positive integer.");
            RuleFor(x => x.uploadedById)
                .GreaterThan(0).WithMessage("Uploader ID must be a positive integer.");
            RuleFor(x => x.FormFile)
                .NotNull().WithMessage("File must be provided.")
                .Must(file => file != null && (file.ContentType == "application/pdf"
                       || file.ContentType == "application/msword"
                       || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                       || file.ContentType == "text/plain"))
                .WithMessage("Only PDF, DOC, DOCX, and TXT file formats are allowed.")
                .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
                .Must(file => file.Length <= 10 * 1024 * 1024).WithMessage("File size must not exceed 10 MB.");
        }
    }
}
