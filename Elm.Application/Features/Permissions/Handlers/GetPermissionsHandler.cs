using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class GetPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, Result<IEnumerable<PermissionDto>>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.Permissions> _permissionRepository;
        private readonly IMapper _mapper;
        public GetPermissionsHandler(IGenericRepository<Elm.Domain.Entities.Permissions> permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetAllAsync();
            var permissionDtos = _mapper.Map<List<PermissionDto>>(permissions);
            return Result<IEnumerable<PermissionDto>>.Success(permissionDtos.AsEnumerable());
        }
    }
}
