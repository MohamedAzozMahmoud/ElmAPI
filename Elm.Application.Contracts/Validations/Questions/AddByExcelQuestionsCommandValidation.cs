using Elm.Application.Contracts.Features.Questions.Commands;
using FluentValidation;

namespace Elm.Application.Contracts.Validations.Questions
{
    public sealed class AddByExcelQuestionsCommandValidation : AbstractValidator<AddByExcelQuestionsCommand>
    {
        public AddByExcelQuestionsCommandValidation()
        {
            RuleFor(x => x.questionBankId)
                .GreaterThan(0).WithMessage("Question Bank Id must be greater than zero.");
            RuleFor(x => x.ExcelFile)
                .NotNull().WithMessage("Excel file must be provided.")
                .Must(file => file.Length > 0).WithMessage("Excel file cannot be empty.")
                .Must(f => f.Length < 10485760).WithMessage("Excel file must be less than 10 MB.");
        }
    }
}
