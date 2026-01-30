using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Domain.Entities.Notifications>, INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadNotificationsCountAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.AppUserId == userId && !n.IsRead)
                .CountAsync();
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .AsNoTracking() // للسرعة وتقليل استهلاك الذاكرة
                .Where(n => n.AppUserId == userId)
                .OrderBy(n => n.IsRead) // غير المقروء (false/0) يظهر أولاً
                .ThenByDescending(n => n.CreatedAt) // ثم الأحدث أولاً
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }


        public async Task<bool> MarkAllNotificationsAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.AppUserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
