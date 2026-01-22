namespace Elm.Domain.Entities
{
    public class Permissions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
        public ICollection<UserPermissions> UserPermissions { get; set; } = new HashSet<UserPermissions>();
    }
}
