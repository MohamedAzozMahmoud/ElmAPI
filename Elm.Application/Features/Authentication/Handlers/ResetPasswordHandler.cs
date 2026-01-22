using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;
        public ResetPasswordHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return Result<bool>.Failure("User not found.", 404);
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (resetPassResult.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            else
            {
                var errors = string.Join(", ", resetPassResult.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"Failed to reset password. Errors: {errors}");
            }
        }
    }
}
