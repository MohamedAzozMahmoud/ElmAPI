using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermissions>
    {
        public Task<bool> AddRolePermissionAsync(RolePermissions rolePermissions);
        public Task<bool> DeleteRolePermissionAsync(string roleId, int PermissionId);
        public Task<List<GetPermissionsDto>> GetPermissionsByRoleNameAsync(string roleId);
    }
}
