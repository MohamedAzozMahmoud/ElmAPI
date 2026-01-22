using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Queries
{
    public record ExportTemplateForQuestionsQuery
        (int QuestionBankId) : IRequest<Result<MemoryStream>>;
}
