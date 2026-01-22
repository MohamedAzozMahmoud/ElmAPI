using System.Text.Json.Serialization;

namespace Elm.Application.Contracts.Features.Authentication.DTOs
{
    public record AuthModelDto
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
