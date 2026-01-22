using Elm.Application.Contracts.Features.Questions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Queries
{
    public record GetAllQuestionsQuery(int questionBankId) : IRequest<Result<List<QuestionsDto>>>;
}
