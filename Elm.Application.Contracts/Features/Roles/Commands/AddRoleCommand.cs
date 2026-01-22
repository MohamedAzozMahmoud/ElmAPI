using MediatR;

namespace Elm.Application.Contracts.Features.Roles.Commands
{
    public record AddRoleCommand(string RoleName) : IRequest<Result<bool>>;
}
