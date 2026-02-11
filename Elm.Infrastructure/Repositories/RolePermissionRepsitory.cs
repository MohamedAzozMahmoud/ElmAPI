using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class RolePermissionRepsitory : GenericRepository<RolePermissions>, IRolePermissionRepository
    {
        private readonly AppDbContext context;
        public RolePermissionRepsitory(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<bool> AddRolePermissionAsync(RolePermissions rolePermissions)
        {
            var existingRolePermission = await FindAsync(rolePermissions.RoleId, rolePermissions.PermissionId);
            if (existingRolePermission != null)
            {
                return false; // RolePermission already exists
            }
            await context.RolePermissions.AddAsync(rolePermissions);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteRolePermissionAsync(string roleId, int PermissionId)
        {
            var rolePermission = await FindAsync(roleId, PermissionId);
            if (rolePermission != null)
            {
                context.RolePermissions.Remove(rolePermission);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<List<GetPermissionsDto>> GetPermissionsByRoleNameAsync(string roleId)
        {
            return await context.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleId == roleId)
                .Select(x => new GetPermissionsDto
                {
                    PermissionId = x.PermissionId,
                    PermissionName = x.Permission.Name
                })
                .ToListAsync();
        }

        private async Task<RolePermissions> FindAsync(string roleId, int PermissionId)
        {
            return await context.RolePermissions
                .SingleOrDefaultAsync(x => x.RoleId == roleId
                                && x.PermissionId == PermissionId);
        }
    }
}
