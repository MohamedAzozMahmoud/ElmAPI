using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> userManager;
        public DeactivateUserCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<bool>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<bool>.Failure("المستخدم غير موجود");
            if (!user.IsActived)
                return Result<bool>.Failure("المستخدم معطل بالفعل");
            user.IsActived = false;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في تعطيل المستخدم");
            return Result<bool>.Success(true);
        }
    }
}
