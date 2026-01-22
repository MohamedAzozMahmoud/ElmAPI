namespace Elm.Application.Contracts.Features.Department.DTOs
{
    public record GetDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPaid { get; set; }
        public string Type { get; set; }
    }
}
