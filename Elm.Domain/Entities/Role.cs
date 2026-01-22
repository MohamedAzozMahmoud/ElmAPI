using Microsoft.AspNetCore.Identity;

namespace Elm.Domain.Entities
{
    public class Role : IdentityRole
    {
        override public string Id { get; set; } = Guid.NewGuid().ToString();
        public ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
