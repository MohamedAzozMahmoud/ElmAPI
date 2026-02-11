namespace Elm.Application.Contracts.Const
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Leader = "Leader";
        public static bool IsValidRole(string role) =>
            role == Admin || role == Doctor || role == Leader;
    }
}
