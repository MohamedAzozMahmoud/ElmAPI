using MediatR;

namespace Elm.Application.Contracts.Features.Roles.Queries
{
    public record GetRolesByUserIdQuery(string userId) : IRequest<Result<IEnumerable<string>>>;
}
