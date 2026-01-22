namespace Elm.Application.Contracts.Features.University.DTOs
{
    public record UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
