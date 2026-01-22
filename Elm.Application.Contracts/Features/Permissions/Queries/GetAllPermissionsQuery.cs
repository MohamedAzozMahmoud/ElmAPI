using Elm.Application.Contracts.Features.Permissions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Queries
{
    public record GetAllPermissionsQuery() : IRequest<Result<IEnumerable<PermissionDto>>>;

}
