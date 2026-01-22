using Elm.Application.Contracts.Features.Questions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Queries
{
    public record GetQuestionByIdQuery(int id) : IRequest<Result<QuestionsDto>>;
}
