using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand, Result<PermissionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Permissions> repository;
        private readonly IMapper mapper;

        public UpdatePermissionHandler(IGenericRepository<Elm.Domain.Entities.Permissions> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<PermissionDto>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await repository.GetByIdAsync(request.Id);
            if (permission == null)
            {
                return Result<PermissionDto>.Failure("Permission not found", 404);
            }
            permission.Name = request.Name;
            await repository.UpdateAsync(permission);
            var permissionDto = mapper.Map<PermissionDto>(permission);
            return Result<PermissionDto>.Success(permissionDto);
        }
    }
}
