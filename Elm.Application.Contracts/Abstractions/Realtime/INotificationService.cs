namespace Elm.Application.Contracts.Abstractions.Realtime
{
    public interface INotificationService
    {
        Task SendNotificationToUser(string userId, string message, string title);
    }
}
