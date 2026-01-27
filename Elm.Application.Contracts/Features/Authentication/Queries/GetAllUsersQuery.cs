using Elm.Application.Contracts.Features.Authentication.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Queries
{
    public record GetAllUsersQuery(string role) : IRequest<Result<IEnumerable<UserDto>>>;
}
