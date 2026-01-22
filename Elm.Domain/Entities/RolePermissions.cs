namespace Elm.Domain.Entities
{
    public class RolePermissions
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
