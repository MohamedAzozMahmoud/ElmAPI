using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class AddUserPermissionHandler : IRequestHandler<AddUserPermissionCommand, Result<bool>>
    {
        private readonly IUserPerissionRepsitory repository;
        private readonly IGenericRepository<Domain.Entities.Permissions> permissionRepository;
        private readonly UserManager<AppUser> userManager;

        public AddUserPermissionHandler(IUserPerissionRepsitory repository,
            IGenericRepository<Domain.Entities.Permissions> permissionRepository, UserManager<AppUser> userManager)
        {
            this.repository = repository;
            this.permissionRepository = permissionRepository;
            this.userManager = userManager;

        }
        public async Task<Result<bool>> Handle(AddUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.userName);
            if (user == null)
            {
                return Result<bool>.Failure("User not found", 404);
            }
            var permission = await permissionRepository.FindAsync(p => p.Name == request.permissionName);
            if (permission == null)
            {
                return Result<bool>.Failure("Permission not found", 404);
            }
            await repository.AddUserPermissionAsync(new UserPermissions
            {
                AppUserId = user.Id,
                PermissionId = permission.Id
            });
            return Result<bool>.Success(true);
        }
    }
}
