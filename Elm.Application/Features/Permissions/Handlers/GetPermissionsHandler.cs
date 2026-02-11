using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class GetPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, Result<IEnumerable<PermissionDto>>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.Permissions> _permissionRepository;
        private readonly MappingProvider _mapping;
        public GetPermissionsHandler(IGenericRepository<Elm.Domain.Entities.Permissions> permissionRepository, MappingProvider mapping)
        {
            _permissionRepository = permissionRepository;
            _mapping = mapping;
        }
        public async Task<Result<IEnumerable<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetAllAsync();
            var permissionDtos = _mapping.MapToDtoList(permissions);
            return Result<IEnumerable<PermissionDto>>.Success(permissionDtos);
        }
    }
}
