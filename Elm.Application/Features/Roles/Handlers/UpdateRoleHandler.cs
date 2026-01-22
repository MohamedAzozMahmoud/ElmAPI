using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{
    public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Result<bool>>
    {
        private readonly RoleManager<Role> roleManager;
        public UpdateRoleHandler(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.oldName);
            if (role == null)
            {
                return Result<bool>.Failure("Role not found.");
            }
            role.Name = request.newName;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
