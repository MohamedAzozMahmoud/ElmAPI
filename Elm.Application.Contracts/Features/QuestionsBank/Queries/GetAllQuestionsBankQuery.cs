using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.QuestionsBank.Queries
{
    public record GetAllQuestionsBankQuery(int curriculumId) : IRequest<Result<List<QuestionsBankDto>>>;
}
