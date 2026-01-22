using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class GetAllRolePermissionsHandler : IRequestHandler<GetAllRolePermissionsQuery, Result<IEnumerable<GetPermissionsDto>>>
    {
        private readonly IRolePermissionRepository repository;
        public GetAllRolePermissionsHandler(IRolePermissionRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result<IEnumerable<GetPermissionsDto>>> Handle(GetAllRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var rolePermissions = await repository.GetPermissionsByRoleIdAsync(request.roleId);
            return Result<IEnumerable<GetPermissionsDto>>.Success(rolePermissions);
        }
    }
}
