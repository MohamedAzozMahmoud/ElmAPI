using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record DeleteRolePermissionCommand(
        string roleName,
        string permissionName
    ) : IRequest<Result<bool>>;
}
