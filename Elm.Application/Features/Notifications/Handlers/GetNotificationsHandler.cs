using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Application.Contracts.Features.Notifications.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Notifications.Handlers
{
    public sealed class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, Result<List<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Result<List<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(request.UserId);
            return Result<List<NotificationDto>>.Success(notifications);
        }
    }
}
