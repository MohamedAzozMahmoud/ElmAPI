namespace Elm.Application.Contracts.Features.University.DTOs
{
    public record UniversityDetialsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; }
    }
}
