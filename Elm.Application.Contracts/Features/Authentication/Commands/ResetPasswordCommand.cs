using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record ResetPasswordCommand(string UserName, string Token, string NewPassword) : IRequest<Result<bool>>;
}
