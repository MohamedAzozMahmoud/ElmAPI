using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{
    public sealed class AddRoleHandler : IRequestHandler<AddRoleCommand, Result<bool>>
    {
        private readonly RoleManager<Role> roleManager;
        public AddRoleHandler(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<bool>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            Role role = new Role();
            role.Name = request.RoleName;
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
