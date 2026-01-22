using MediatR;

namespace Elm.Application.Contracts.Features.Notifications.Queries
{
    public record GetUnReadNotificationsCountQuery(string UserId) : IRequest<Result<int>>;
}
