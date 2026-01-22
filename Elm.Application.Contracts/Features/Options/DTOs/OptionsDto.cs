namespace Elm.Application.Contracts.Features.Options.DTOs
{
    public record OptionsDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
