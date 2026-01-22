using Elm.Application.Contracts.Features.Notifications.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Notifications.Queries
{
    public record GetNotificationsQuery(string UserId) : IRequest<Result<List<NotificationDto>>>;
}
