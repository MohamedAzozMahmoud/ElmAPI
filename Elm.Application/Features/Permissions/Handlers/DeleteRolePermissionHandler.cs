using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class DeleteRolePermissionHandler : IRequestHandler<DeleteRolePermissionCommand, Result<bool>>
    {
        private readonly IRolePermissionRepository repository;
        private readonly IGenericRepository<Domain.Entities.Permissions> permissionRepository;
        private readonly RoleManager<Role> roleManager;
        public DeleteRolePermissionHandler(IRolePermissionRepository repository,
            IGenericRepository<Domain.Entities.Permissions> permissionRepository,
            RoleManager<Role> manager)
        {
            this.repository = repository;
            this.permissionRepository = permissionRepository;
            this.roleManager = manager;
        }
        public async Task<Result<bool>> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByNameAsync(request.roleName);
            if (role == null)
            {
                return Result<bool>.Failure("Role not found", 404);
            }
            var permission = await permissionRepository.FindAsync(p => p.Name == request.permissionName);
            if (permission == null)
            {
                return Result<bool>.Failure("Permission not found", 404);
            }
            await repository.DeleteRolePermissionAsync(role.Id, permission.Id);
            return Result<bool>.Success(true);
        }
    }
}
