using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class GetAllRolePermissionsHandler : IRequestHandler<GetAllRolePermissionsQuery, Result<IEnumerable<GetPermissionsDto>>>
    {
        private readonly IRolePermissionRepository repository;
        private readonly RoleManager<Role> roleManager;
        public GetAllRolePermissionsHandler(IRolePermissionRepository repository, RoleManager<Role> roleManager)
        {
            this.repository = repository;
            this.roleManager = roleManager;
        }
        public async Task<Result<IEnumerable<GetPermissionsDto>>> Handle(GetAllRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var roleExists = await roleManager.FindByNameAsync(request.roleName);
            if (roleExists == null)
            {
                return Result<IEnumerable<GetPermissionsDto>>.NotFound($"Role with name '{request.roleName}' does not exist.");
            }
            var rolePermissions = await repository.GetPermissionsByRoleNameAsync(roleExists.Id);
            return Result<IEnumerable<GetPermissionsDto>>.Success(rolePermissions);
        }
    }
}
