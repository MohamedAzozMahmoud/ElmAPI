using Elm.Application.Contracts.Features.Permissions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Queries
{
    public record GetAllUserPermissionsQuery(string userId) : IRequest<Result<IEnumerable<GetPermissionsDto>>>;

}
