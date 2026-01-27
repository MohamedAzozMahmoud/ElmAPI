namespace Elm.Application.Contracts.Features.Authentication.DTOs
{
    public record UserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
