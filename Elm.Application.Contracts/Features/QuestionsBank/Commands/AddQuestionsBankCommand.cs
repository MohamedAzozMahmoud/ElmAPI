using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.QuestionsBank.Commands
{
    public record AddQuestionsBankCommand(string name, int curriculumId) : IRequest<Result<QuestionsBankDto>>;
}
