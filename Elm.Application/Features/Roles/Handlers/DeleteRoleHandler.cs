using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{
    public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, Result<bool>>
    {
        private readonly RoleManager<Role> roleManager;
        public DeleteRoleHandler(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.Name);
            if (role == null)
            {
                return Result<bool>.Failure("Role not found.");
            }
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
