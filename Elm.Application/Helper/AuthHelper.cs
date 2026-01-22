using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Elm.Application.Helper
{
    public class AuthHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JWT jwt;
        public AuthHelper(UserManager<AppUser> userManager, IOptions<JWT> jwtOptions)
        {
            _userManager = userManager;
            jwt = jwtOptions.Value;
        }
        public async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var userClaims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name,user.UserName)
            }
            .Union(roleClaims)
            .Union(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            return new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(jwt.Duration),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );
        }
        public static RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            RandomNumberGenerator.Fill(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(3),
                CreatedOn = DateTime.UtcNow
            };
        }


    }
}
