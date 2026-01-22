namespace Elm.Application.Contracts.Features.College.DTOs
{
    public record GetCollegeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagName { get; set; }
    }
}
