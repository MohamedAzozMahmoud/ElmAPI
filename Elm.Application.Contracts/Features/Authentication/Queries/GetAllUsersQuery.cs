using Elm.Application.Contracts.Features.Authentication.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Queries
{
    public record GetAllLeadersQuery(int pageSize, int pageNumber) : IRequest<Result<IEnumerable<LeaderDto>>>;
    public record GetAllUsersQuery(string role) : IRequest<Result<IEnumerable<UserDto>>>;
}
