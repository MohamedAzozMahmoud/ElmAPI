namespace Elm.Domain.Entities
{
    //[Owned]
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && DateTime.UtcNow < ExpiresOn;

        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
    }
}
