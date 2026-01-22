using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    //INotificationRepository
    public interface INotificationRepository : IGenericRepository<Notifications>
    {
        public Task<bool> MarkNotificationAsReadAsync(int notificationId);
        public Task<bool> MarkAllNotificationsAsReadAsync(string userId);
        public Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
        public Task<int> GetUnreadNotificationsCountAsync(string userId);
        public Task<bool> DeleteNotificationAsync(int notificationId);

    }
}
