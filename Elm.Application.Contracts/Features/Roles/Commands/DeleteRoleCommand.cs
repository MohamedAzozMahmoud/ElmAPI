using MediatR;

namespace Elm.Application.Contracts.Features.Roles.Commands
{
    public record DeleteRoleCommand(string Name) : IRequest<Result<bool>>;
}
