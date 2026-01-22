using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class DeletePermissionHandler : IRequestHandler<DeletePermissionCommand, Result<bool>>
    {
        private readonly IGenericRepository<Elm.Domain.Entities.Permissions> permissionRepository;
        public DeletePermissionHandler(IGenericRepository<Elm.Domain.Entities.Permissions> permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        public async Task<Result<bool>> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            await permissionRepository.DeleteAsync(request.Id);
            return Result<bool>.Success(true);
        }
    }
}
