using MediatR;

namespace Elm.Application.Contracts.Features.Authentication.Commands
{
    public record ChangePasswordCommand(string UserId, string currentPassword, string newPassword, string confidentialPassword) : IRequest<Result<bool>>;
}
