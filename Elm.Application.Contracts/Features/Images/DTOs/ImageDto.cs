namespace Elm.Application.Contracts.Features.Images.DTOs
{
    public record ImageDto
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
