namespace Elm.Application.Contracts.Features.Subject.DTOs
{
    public record GetSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
