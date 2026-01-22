using Elm.Application.Contracts.Features.Notifications.DTOs;

namespace Elm.Application.Contracts.Abstractions.Realtime
{
    public interface INotificationClient
    {
        Task ReceiveNotification(NotificationDto notification);
    }
}
