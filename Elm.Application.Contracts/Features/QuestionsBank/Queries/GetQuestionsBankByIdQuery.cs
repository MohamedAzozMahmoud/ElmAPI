using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.QuestionsBank.Queries
{
    public record GetQuestionsBankByIdQuery(int id) : IRequest<Result<QuestionsBankDto>>;
}
