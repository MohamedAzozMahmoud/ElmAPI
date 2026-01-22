namespace Elm.Application.Contracts.Features.Files.DTOs
{
    public record FileView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageName { get; set; }
        public string DoctorRatedName { get; set; }
        public string? comment { get; set; }
        public DateTime? RatedAt { get; set; }
    }
}
