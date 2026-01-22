using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record AddRolePermissionCommand(
        string roleName,
        string permissionName
    ) : IRequest<Result<bool>>;
}
