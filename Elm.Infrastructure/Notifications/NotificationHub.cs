using Elm.Application.Contracts.Abstractions.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Elm.Infrastructure.Notifications
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            return base.OnConnectedAsync();
        }
    }
}
