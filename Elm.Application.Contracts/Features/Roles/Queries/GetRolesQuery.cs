using Elm.Application.Contracts.Features.Roles.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Roles.Queries
{
    public record GetRolesQuery() : IRequest<Result<IEnumerable<RoleDto>>>;
}
