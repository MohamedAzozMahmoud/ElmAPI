namespace Elm.Application.Contracts.Features.Options.DTOs
{
    public record OptionTestDto
    {
        public int Id { get; init; }
        public string Content { get; init; } = null!;
        public bool IsSelected { get; set; }
    }
}
