using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class AddRolePermissionHandler : IRequestHandler<AddRolePermissionCommand, Result<bool>>
    {
        private readonly IRolePermissionRepository repository;
        private readonly IGenericRepository<Elm.Domain.Entities.Permissions> permission;
        private readonly RoleManager<Role> roleManager;
        public AddRolePermissionHandler(IRolePermissionRepository repository, RoleManager<Role> roleManager, IGenericRepository<Elm.Domain.Entities.Permissions> permission)
        {
            this.repository = repository;
            this.roleManager = roleManager;
            this.permission = permission;
        }
        public async Task<Result<bool>> Handle(AddRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.roleName);
            if (role == null)
            {
                return Result<bool>.Failure("Role not found", 404);
            }
            var permissionExists = await permission.FindAsync(x => x.Name == request.permissionName);
            if (permissionExists == null)
            {
                return Result<bool>.Failure("Permission not found", 404);
            }
            await repository.AddRolePermissionAsync(new RolePermissions { PermissionId = permissionExists.Id, RoleId = role.Id });
            return Result<bool>.Success(true);
        }
    }
}
