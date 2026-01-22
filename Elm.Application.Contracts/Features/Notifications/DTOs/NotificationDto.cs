namespace Elm.Application.Contracts.Features.Notifications.DTOs
{
    public record NotificationDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
