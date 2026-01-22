using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Commands
{
    public record AddByExcelQuestionsCommand(int questionBankId, Stream ExcelFile) : IRequest<Result<bool>>;
}
