using MediatR;

namespace Elm.Application.Contracts.Features.Notifications.Commands
{
    public record MarkNotificationAsReadCommand(int NotificationId) : IRequest<Result<bool>>;
}
