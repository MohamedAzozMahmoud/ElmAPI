namespace Elm.Application.Contracts.Features.Department.DTOs
{
    public record DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; }
        public string Type { get; set; }
        public int CollegeId { get; set; }
    }
}
