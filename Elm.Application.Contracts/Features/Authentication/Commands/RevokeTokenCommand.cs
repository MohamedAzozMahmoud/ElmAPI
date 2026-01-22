using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record RevokeTokenCommand(string Token) : IRequest<Result<bool>>;
}
