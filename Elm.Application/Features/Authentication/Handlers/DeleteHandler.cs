using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class DeleteHandler : IRequestHandler<DeleteCommand, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;
        public DeleteHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<bool>.Failure("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"Failed to delete user. Errors: {errors}");
            }
        }
    }
}
