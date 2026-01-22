namespace Elm.Application.Contracts.Features.Notifications.DTOs
{
    public record GetAllNotificationsDto
    {
        public string Title { get; init; }
        public string Message { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
