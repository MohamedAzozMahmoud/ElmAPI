using Elm.Application.Contracts;
using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class RegisterStudentHandler : IRequestHandler<RegisterLeaderCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> userManager;
        public RegisterStudentHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<bool>> Handle(RegisterLeaderCommand request, CancellationToken cancellationToken)
        {
            var Found = await userManager.FindByNameAsync(request.UserName);
            if (Found != null)
                return Result<bool>.Failure("المستخدم موجود بالفعل");
            var user = new AppUser
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Student = new Student
                {
                    YearId = request.YearId,
                    DepartmentId = request.DepartmentId
                },
                IsActived = true
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في إنشاء المستخدم");
            await userManager.AddToRoleAsync(user, UserRoles.Leader);
            return Result<bool>.Success(true);
        }
    }
}
