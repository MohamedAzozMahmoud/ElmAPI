using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class AddPermissionHandler : IRequestHandler<AddPermissionCommand, Result<PermissionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Permissions> repository;
        private readonly IMapper mapper;
        public AddPermissionHandler(IGenericRepository<Elm.Domain.Entities.Permissions> repository, IMapper _mapper)
        {
            this.repository = repository;
            this.mapper = _mapper;
        }
        public async Task<Result<PermissionDto>> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = new Domain.Entities.Permissions
            {
                Name = request.Name
            };
            var addedPermission = await repository.AddAsync(permission);
            var permissionDto = mapper.Map<PermissionDto>(addedPermission);
            return Result<PermissionDto>.Success(permissionDto);
        }
    }
}
