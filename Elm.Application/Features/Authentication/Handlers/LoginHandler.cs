using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Helper;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<AuthModelDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<JWT> jwt;

        public LoginHandler(UserManager<AppUser> userManager, IOptions<JWT> options)
        {
            _userManager = userManager;
            jwt = options;
        }
        public async Task<Result<AuthModelDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid || user == null || !user.IsActived)
                return Result<AuthModelDto>.Failure("كلمة المرور او اسم المستخدم غير صحيح");

            var roles = await _userManager.GetRolesAsync(user);
            var authHelper = new AuthHelper(_userManager, jwt);
            var jwtToken = await authHelper.CreateJwtToken(user);
            var newRefreshToken = AuthHelper.GenerateRefreshToken();
            var authModel = new AuthModelDto
            {
                IsAuthenticated = true,
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Roles = roles.ToList(),
                ExpiresOn = jwtToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
            user.RefreshTokens.Add(newRefreshToken);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded
                ? Result<AuthModelDto>.Success(authModel)
                : Result<AuthModelDto>.Failure("حدث خطأ أثناء تحديث البيانات");
        }

    }
}
