using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Domain.Entities;

namespace Elm.Application.Contracts.Repositories
{
    public interface IUserPerissionRepsitory : IGenericRepository<UserPermissions>
    {
        public Task<bool> AddUserPermissionAsync(UserPermissions userPermissions);
        public Task<bool> DeleteUserPermissionAsync(string userId, int PermissionId);
        public Task<List<GetPermissionsDto>> GetUserPermissionsByUserIdAsync(string userId);
    }
}
