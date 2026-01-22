using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Helper;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthModelDto>>
    {
        private readonly IOptions<JWT> jwt;
        private readonly UserManager<AppUser> _userManager;
        public RefreshTokenHandler(IOptions<JWT> options, UserManager<AppUser> userManager)
        {
            jwt = options;
            _userManager = userManager;
        }
        public async Task<Result<AuthModelDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var authModel = new AuthModelDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

            if (user == null)
            {
                return Result<AuthModelDto>.Failure("المستخدم غير موجود");
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == request.Token);

            if (!refreshToken.IsActive)
            {
                return Result<AuthModelDto>.Failure("رمز التحديث غير نشط");
            }

            if (refreshToken.IsActive)
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
            }
            var authHelper = new AuthHelper(_userManager, jwt);

            var newRefreshToken = AuthHelper.GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtToken = await authHelper.CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            authModel.FullName = user.FullName;
            authModel.UserId = user.Id;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtToken.ValidTo;
            authModel.IsAuthenticated = true;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
            ? Result<AuthModelDto>.Success(authModel)
            : Result<AuthModelDto>.Failure("حدث خطأ أثناء تحديث البيانات");
        }
    }
}
