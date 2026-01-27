using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Application.Contracts.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize]
    [EnableRateLimiting("UserRolePolicy")]
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
        [HttpPut]
        [Route("MarkAllAsRead")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> MarkAllAsRead([FromBody] MarkAllNotificationsAsReadCommand command)
        => HandleResult(await mediator.Send(command));

        // PUT: api/Notifications/MarkAsRead/{notificationId}
        [HttpPut]
        [Route("MarkAsRead/{notificationId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> MarkAsRead([FromRoute] int notificationId)
            => HandleResult(await mediator.Send(new MarkNotificationAsReadCommand(notificationId)));

        // GET: api/Notifications/UserNotifications/{userId}
        [HttpGet]
        [Route("UserNotifications/{userId}")]
        [ProducesResponseType(typeof(List<NotificationDto>), 200)]
        public async Task<IActionResult> GetUserNotifications([FromRoute] string userId)
            => HandleResult(await mediator.Send(new GetNotificationsQuery(userId)));

        // GET: api/Notifications/UnreadCount/{userId}
        [HttpGet]
        [Route("UnreadCount/{userId}")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetUnreadNotificationsCount([FromRoute] string userId)
            => HandleResult(await mediator.Send(new GetUnReadNotificationsCountQuery(userId)));

        // DELETE: api/Notifications/Delete/{notificationId}
        [HttpDelete]
        [Route("Delete/{notificationId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteNotification([FromRoute] int notificationId)
            => HandleResult(await mediator.Send(new DeleteNotificationCommand(notificationId)));


    }
}
