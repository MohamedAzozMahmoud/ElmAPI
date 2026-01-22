using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Notifications.Handlers
{
    public sealed class GetUnReadNotificationsCountHandler : IRequestHandler<GetUnReadNotificationsCountQuery, Result<int>>
    {
        private readonly INotificationRepository _notificationRepository;
        public GetUnReadNotificationsCountHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Result<int>> Handle(GetUnReadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _notificationRepository.GetUnreadNotificationsCountAsync(request.UserId);
            return Result<int>.Success(count);
        }
    }
}
