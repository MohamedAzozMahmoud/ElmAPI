using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Notifications.Handlers
{
    public sealed class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsReadCommand, Result<bool>>
    {
        private readonly INotificationRepository _notificationRepository;
        public MarkNotificationAsReadHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Result<bool>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var success = await _notificationRepository.MarkNotificationAsReadAsync(request.NotificationId);
            return Result<bool>.Success(success);
        }
    }
}
