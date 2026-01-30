using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Repositories
{
    public class UserPerissionRepsitory : GenericRepository<UserPermissions>, IUserPerissionRepsitory
    {
        private readonly AppDbContext context;
        public UserPerissionRepsitory(AppDbContext _context) : base(_context)
        {
            context = _context;
        }


        public async Task<bool> AddUserPermissionAsync(UserPermissions userPermissions)
        {
            var existingPermission = await FindAsync(userPermissions.AppUserId, userPermissions.PermissionId);
            if (existingPermission != null)
            {
                return false; // Permission already exists for the user
            }
            await context.UserPermissions.AddAsync(userPermissions);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteUserPermissionAsync(string userId, int PermissionId)
        {
            var userPermission = await FindAsync(userId, PermissionId);
            if (userPermission != null)
            {
                context.UserPermissions.Remove(userPermission);
                var result = await context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<List<GetPermissionsDto>> GetUserPermissionsByUserIdAsync(string userId)
        {
            return await context.UserPermissions
                .AsNoTracking()
                .Where(up => up.AppUserId == userId)
                .Select(x => new GetPermissionsDto
                {
                    PermissionId = x.PermissionId,
                    PermissionName = x.Permission.Name
                })
                .ToListAsync();
        }

        private async Task<UserPermissions> FindAsync(string userId, int PermissionId)
        {
            return await context.UserPermissions.SingleOrDefaultAsync(x => x.AppUserId == userId
                                && x.PermissionId == PermissionId);
        }

    }
}
