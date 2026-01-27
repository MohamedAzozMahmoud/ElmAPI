namespace Elm.Application.Contracts.Const
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Student = "Student";
        public static bool IsValidRole(string role) =>
            role == Admin || role == Doctor || role == Student;
    }
}
