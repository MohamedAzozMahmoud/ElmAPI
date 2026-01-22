namespace Elm.Application.Contracts.Features.Options.DTOs
{
    public record AddOptionsDto
    {
        public string Content { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
