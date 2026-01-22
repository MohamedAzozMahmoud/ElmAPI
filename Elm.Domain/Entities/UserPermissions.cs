namespace Elm.Domain.Entities
{
    public class UserPermissions
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser User { get; set; }
        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
