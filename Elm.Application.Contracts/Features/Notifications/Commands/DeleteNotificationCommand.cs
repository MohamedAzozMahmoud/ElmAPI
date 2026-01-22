using MediatR;

namespace Elm.Application.Contracts.Features.Notifications.Commands
{
    public record DeleteNotificationCommand(int NotificationId) : IRequest<Result<bool>>;
}
