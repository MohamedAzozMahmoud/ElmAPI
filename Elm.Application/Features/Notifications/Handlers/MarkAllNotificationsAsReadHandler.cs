using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Notifications.Handlers
{
    public sealed class MarkAllNotificationsAsReadHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, Result<bool>>
    {
        private readonly INotificationRepository _notificationRepository;
        public MarkAllNotificationsAsReadHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Result<bool>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var success = await _notificationRepository.MarkAllNotificationsAsReadAsync(request.UserId);
            return Result<bool>.Success(success);
        }
    }
}
