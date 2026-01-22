namespace Elm.Application.Contracts.Features.Curriculum.DTOs
{
    public record CurriculumDto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int YearId { get; set; }
        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
    }
}
