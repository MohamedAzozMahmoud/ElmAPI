using MediatR;

namespace Elm.Application.Contracts.Features.QuestionsBank.Commands
{
    public record UpdateQuestionsBankCommand(int id, string name, int curriculumId) : IRequest<Result<bool>>;
}
