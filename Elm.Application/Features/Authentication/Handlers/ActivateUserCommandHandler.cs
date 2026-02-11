using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> userManager;
        public ActivateUserCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<bool>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<bool>.Failure("المستخدم غير موجود");
            if (user.IsActived)
                return Result<bool>.Failure("المستخدم مفعل بالفعل");
            user.IsActived = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في تفعيل المستخدم");
            return Result<bool>.Success(true);
        }
    }
}
