using Elm.Application.Contracts;
using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class RegisterDoctorHandler : IRequestHandler<RegisterDoctorCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> userManager;
        public RegisterDoctorHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<bool>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
        {
            var Found = await userManager.FindByNameAsync(request.UserName);
            if (Found != null)
                return Result<bool>.Failure("المستخدم موجود بالفعل");
            var user = new AppUser
            {
                UserName = request.UserName,
                FullName = request.FullName,
                IsActived = true,
                Doctor = new Doctor()
                {
                    Title = request.Title
                }
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في إنشاء المستخدم");
            await userManager.AddToRoleAsync(user, UserRoles.Doctor);
            return Result<bool>.Success(true);
        }
    }
}
