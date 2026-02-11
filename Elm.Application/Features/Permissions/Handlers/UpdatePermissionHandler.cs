using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand, Result<bool>>
    {
        private readonly IGenericRepository<Domain.Entities.Permissions> repository;
        public UpdatePermissionHandler(IGenericRepository<Elm.Domain.Entities.Permissions> repository)
        {
            this.repository = repository;
        }
        public async Task<Result<bool>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await repository.GetByIdAsync(request.Id);
            if (permission == null)
            {
                return Result<bool>.Failure("Permission not found", 404);
            }
            permission.Name = request.Name;
            var result = await repository.UpdateAsync(permission);
            if (!result)
            {
                return Result<bool>.Failure("Failed to update permission");
            }
            return Result<bool>.Success(result);
        }
    }
}
