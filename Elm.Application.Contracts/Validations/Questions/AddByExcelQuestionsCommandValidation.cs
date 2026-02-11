using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddByExcelQuestionsCommandValidation : AbstractValidator<AddByExcelQuestionsCommand>
    {
        public AddByExcelQuestionsCommandValidation()
        {
            RuleFor(x => x.questionBankId)
                .GreaterThan(0).WithMessage("معرف بنك الأسئلة يجب أن يكون أكبر من صفر.");
            RuleFor(x => x.ExcelFile)
                .NotNull().WithMessage("يجب توفير ملف Excel.")
                .Must(file => file.Length > 0).WithMessage("ملف Excel لا يمكن أن يكون فارغًا.")
                .Must(f => f.Length < 3 * 1024 * 1024).WithMessage("يجب أن يكون حجم ملف Excel أقل من 3 ميغابايت.");
        }
    }
}
