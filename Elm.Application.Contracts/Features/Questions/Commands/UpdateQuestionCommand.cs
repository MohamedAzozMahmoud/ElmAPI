using Elm.Application.Contracts.Features.Questions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Commands
{
    public record UpdateQuestionCommand(int Id, string Content, string QuestionType) : IRequest<Result<QuestionsDto>>;
}
