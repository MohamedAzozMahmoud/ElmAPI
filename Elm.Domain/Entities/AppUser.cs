using Microsoft.AspNetCore.Identity;

namespace Elm.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActived { get; set; } = false;
        // Navigation Properties
        public Doctor? Doctor { get; set; }
        public Student? Student { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public ICollection<UserPermissions> UserPermissions { get; set; } = new HashSet<UserPermissions>();
    }
}
