using Elm.Application.Contracts.Abstractions.Settings;
using Elm.Application.Contracts.Features.Files.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Files
{
    public sealed class UploadFileCommandValidation : AbstractValidator<UploadFileCommand>
    {
        private readonly ISettingsService settingsService;

        public UploadFileCommandValidation(ISettingsService _settingsService)
        {
            settingsService = _settingsService;
            var maxStorageBytes = settingsService.GetMaxStorageBytesAsync().GetAwaiter().GetResult();
            RuleFor(x => x.curriculumId)
                .GreaterThan(0).WithMessage("معرّف المنهج يجب أن يكون عددًا صحيحًا موجبًا.");
            RuleFor(x => x.uploadedById)
                .GreaterThan(0).WithMessage("معرّف الطالب يجب أن يكون عددًا صحيحًا موجبًا.");
            RuleFor(x => x.FormFile)
                .NotNull().WithMessage("يجب تقديم الملف.")
                .Must(file => file != null && (file.ContentType == "application/pdf"
                       || file.ContentType == "application/msword"
                       || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                       || file.ContentType == "text/plain"))
                .WithMessage("يُسمح فقط بصيغ الملفات PDF و DOC و DOCX و TXT.")
                .Must(file => file.Length > 0).WithMessage("الملف لا يمكن أن يكون فارغًا.")
                .Must(file => file.Length <= maxStorageBytes).WithMessage($"حجم الملف لا يجب أن يتجاوز {maxStorageBytes / (1024 * 1024)} ميغابايت.");
        }
    }
}
