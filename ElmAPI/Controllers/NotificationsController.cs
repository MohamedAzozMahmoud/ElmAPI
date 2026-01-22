using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ApiBaseController
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // PUT: api/Notifications/MarkAllAsRead
        [HttpPut("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead([FromBody] MarkAllNotificationsAsReadCommand command)
        => HandleResult(await mediator.Send(command));

        // PUT: api/Notifications/MarkAsRead/{notificationId}
        [HttpPut("MarkAsRead/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
            => HandleResult(await mediator.Send(new MarkNotificationAsReadCommand(notificationId)));

        // GET: api/Notifications/UserNotifications/{userId}
        [HttpGet("UserNotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
            => HandleResult(await mediator.Send(new GetNotificationsQuery(userId)));

        // GET: api/Notifications/UnreadCount/{userId}
        [HttpGet("UnreadCount/{userId}")]
        public async Task<IActionResult> GetUnreadNotificationsCount(string userId)
            => HandleResult(await mediator.Send(new GetUnReadNotificationsCountQuery(userId)));

        // DELETE: api/Notifications/Delete/{notificationId}
        [HttpDelete("Delete/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
            => HandleResult(await mediator.Send(new DeleteNotificationCommand(notificationId)));


    }
}
