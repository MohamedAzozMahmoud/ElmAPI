using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Mapper.Elm.Application.Mappers;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class AddPermissionHandler : IRequestHandler<AddPermissionCommand, Result<PermissionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Permissions> repository;
        private readonly MappingProvider mapping;
        public AddPermissionHandler(IGenericRepository<Elm.Domain.Entities.Permissions> repository, MappingProvider mapping)
        {
            this.repository = repository;
            this.mapping = mapping;
        }
        public async Task<Result<PermissionDto>> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = new Domain.Entities.Permissions
            {
                Name = request.Name
            };
            var addedPermission = await repository.AddAsync(permission);
            var permissionDto = mapping.MapToDto(addedPermission);
            return Result<PermissionDto>.Success(permissionDto);
        }
    }
}
