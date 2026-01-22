using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;

        // لاحظ أننا لم نعد بحاجة لـ IMapper أو IOptions<JWT> هنا لأن هذه العملية لا تحتاجهم
        public ChangePasswordHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // 1. البحث عن المستخدم
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<bool>.Failure("لا يوجد مستخدم", 404);

            // 2. التحقق من كلمة المرور الحالية
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.currentPassword);
            if (!isPasswordValid)
                return Result<bool>.Failure("كلمة المرور الحالية غير صحيحة");

            // 3. تغيير كلمة المرور
            var result = await _userManager.ChangePasswordAsync(user, request.currentPassword, request.newPassword);

            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في تغيير كلمة المرور");

            return Result<bool>.Success(true);
        }
    }
}
