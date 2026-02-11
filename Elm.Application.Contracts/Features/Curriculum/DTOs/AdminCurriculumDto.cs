namespace Elm.Application.Contracts.Features.Curriculum.DTOs
{
    public record AdminCurriculumDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string DepartmentName { get; set; }
        public string YearName { get; set; }
        public string DoctorName { get; set; }
    }
}
