using Elm.Application.Contracts.Features.Authentication.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record RefreshTokenCommand(string Token) : IRequest<Result<AuthModelDto>>;
}
