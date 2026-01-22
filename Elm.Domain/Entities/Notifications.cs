namespace Elm.Domain.Entities
{
    public class Notifications
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string AppUserId { get; set; }
        public AppUser User { get; set; }
    }
}
