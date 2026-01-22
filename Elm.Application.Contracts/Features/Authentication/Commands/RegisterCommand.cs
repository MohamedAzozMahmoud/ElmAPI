using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record RegisterCommand(string UserName, string Password, string ConfirmPassword, string FullName) : IRequest<Result<bool>>;
}
