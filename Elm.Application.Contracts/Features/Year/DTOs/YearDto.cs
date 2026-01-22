namespace Elm.Application.Contracts.Features.Year.DTOs
{
    public record YearDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CollegeId { get; set; }
    }
}
