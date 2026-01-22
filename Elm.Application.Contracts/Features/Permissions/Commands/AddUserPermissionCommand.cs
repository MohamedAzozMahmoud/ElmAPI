using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record AddUserPermissionCommand(
        string userName,
        string permissionName
    ) : IRequest<Result<bool>>;
}
