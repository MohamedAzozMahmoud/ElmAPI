using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class DeleteUserPermissionHandler : IRequestHandler<DeleteUserPermissionCommand, Result<bool>>
    {
        private readonly IUserPerissionRepsitory repository;
        private readonly IGenericRepository<Domain.Entities.Permissions> permissionRepository;
        private readonly UserManager<AppUser> userManager;
        public DeleteUserPermissionHandler(IUserPerissionRepsitory repository,
            IGenericRepository<Domain.Entities.Permissions> permissionRepository, UserManager<AppUser> userManager)
        {
            this.repository = repository;
            this.permissionRepository = permissionRepository;
            this.userManager = userManager;
        }
        public async Task<Result<bool>> Handle(DeleteUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await permissionRepository.FindAsync(p => p.Name == request.permissionName);
            if (permission == null)
            {
                return Result<bool>.Failure("Permission not found", 404);
            }
            var user = await userManager.FindByNameAsync(request.userName);
            if (user == null)
            {
                return Result<bool>.Failure("User not found", 404);
            }
            await repository.DeleteUserPermissionAsync(user.Id, permission.Id);
            return Result<bool>.Success(true);
        }
    }
}
