namespace Elm.Application.Contracts.Features.Authentication.DTOs
{
    public record LeaderDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string YearName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActived { get; set; }
    }
}
