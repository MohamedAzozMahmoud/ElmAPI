using Elm.Application.Contracts.Abstractions.Realtime;
using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Elm.Infrastructure.Services.Realtime
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly AppDbContext _context;

        public NotificationService(IHubContext<NotificationHub, INotificationClient> hubContext, AppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task SendNotificationToUser(string userId, string message, string title)
        {
            var notification = new Elm.Domain.Entities.Notifications
            {
                AppUserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Title = title
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            var notificationDto = new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead
            };
            await _hubContext.Clients.User(userId).ReceiveNotification(notificationDto);
        }
    }
}
