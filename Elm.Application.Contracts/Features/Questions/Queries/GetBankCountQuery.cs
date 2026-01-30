using MediatR;

namespace Elm.Application.Contracts.Features.Questions.Queries
{
    public record GetBankCountQuery(int bankId) : IRequest<Result<int>>;
}
