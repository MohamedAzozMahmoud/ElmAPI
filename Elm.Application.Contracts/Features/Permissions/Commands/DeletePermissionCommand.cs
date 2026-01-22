using MediatR;

namespace Elm.Application.Contracts.Features.Permissions.Commands
{
    public record DeletePermissionCommand(int Id) : IRequest<Result<bool>>;
}
