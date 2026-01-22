using Elm.Application.Contracts.Features.Authentication.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record LoginCommand(string UserName, string Password) : IRequest<Result<AuthModelDto>>;
}
