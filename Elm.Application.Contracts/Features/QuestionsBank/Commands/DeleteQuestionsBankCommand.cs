using MediatR;

namespace Elm.Application.Contracts.Features.QuestionsBank.Commands
{
    public record DeleteQuestionsBankCommand(int id) : IRequest<Result<bool>>;
}
