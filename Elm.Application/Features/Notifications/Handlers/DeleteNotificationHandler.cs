using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Notifications.Handlers
{
    public sealed class DeleteNotificationHandler : IRequestHandler<DeleteNotificationCommand, Result<bool>>
    {
        private readonly INotificationRepository _notificationRepository;
        public DeleteNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Result<bool>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var success = await _notificationRepository.DeleteNotificationAsync(request.NotificationId);
            return Result<bool>.Success(success);
        }
    }
}
