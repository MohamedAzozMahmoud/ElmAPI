using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Commands
{
    public record DeleteQuestionCommand(int questionId) : IRequest<Result<bool>>;
}
