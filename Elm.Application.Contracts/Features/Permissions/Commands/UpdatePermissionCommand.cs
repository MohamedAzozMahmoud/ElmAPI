using Elm.Application.Contracts.Features.Permissions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record UpdatePermissionCommand(
        int Id,
        string Name
    ) : IRequest<Result<PermissionDto>>;
}
