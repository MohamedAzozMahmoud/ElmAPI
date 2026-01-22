using Elm.Application.Contracts.Features.Questions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Commands
{
    public record AddQuestionCommand(int questionBankId, AddQuestionsDto QuestionsDto) : IRequest<Result<QuestionsDto>>;
}
