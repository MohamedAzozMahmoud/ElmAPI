using MediatR;

namespace Elm.Application.Contracts.Features.Roles.Commands
{
    public record UpdateRoleCommand(string oldName, string newName) : IRequest<Result<bool>>;
}
