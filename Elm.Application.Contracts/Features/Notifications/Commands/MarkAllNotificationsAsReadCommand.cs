using MediatR;

namespace Elm.Application.Contracts.Features.Notifications.Commands
{
    public record MarkAllNotificationsAsReadCommand(string UserId) : IRequest<Result<bool>>;
}
