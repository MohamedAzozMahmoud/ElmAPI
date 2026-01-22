using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record DeleteCommand(string UserId) : IRequest<Result<bool>>;
}
