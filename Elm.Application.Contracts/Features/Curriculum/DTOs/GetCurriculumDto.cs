namespace Elm.Application.Contracts.Features.Curriculum.DTOs
{
    public record GetCurriculumDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }
}
