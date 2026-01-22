using Elm.Application.Contracts.Features.Permissions.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record AddPermissionCommand(
        string Name
    ) : IRequest<Result<PermissionDto>>;
}
