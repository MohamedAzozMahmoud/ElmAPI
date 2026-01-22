using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Helper;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IOptions<JWT> jwt;
        public RevokeTokenHandler(UserManager<AppUser> userManager, IGenericRepository<RefreshToken> refreshTokenRepository, IOptions<JWT> options)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            jwt = options;
        }
        public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

            if (user == null)
                return Result<bool>.Failure("لا يوجد مستخدم");

            var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == request.Token);

            if (refreshToken == null)
                return Result<bool>.Failure("رمز التحديث غير موجود");

            if (!refreshToken.IsActive)
                return Result<bool>.Failure("رمز التحديث غير نشط");

            if (refreshToken.IsActive)
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
            }
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
            ? Result<bool>.Success(true)
            : Result<bool>.Failure("حدث خطأ أثناء تحديث البيانات");
        }
    }
}
