using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record DeleteUserPermissionCommand(
        string userName,
        string permissionName
    ) : IRequest<Result<bool>>;
}
