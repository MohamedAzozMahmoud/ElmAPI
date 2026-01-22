namespace Elm.Application.Contracts.Features.Files.DTOs
{
    public record FileResponse
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
