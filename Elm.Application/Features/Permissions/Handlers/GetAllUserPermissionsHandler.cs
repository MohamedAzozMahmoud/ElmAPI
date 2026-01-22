using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Permissions.Handlers
{
    public sealed class GetAllUserPermissionsHandler : IRequestHandler<GetAllUserPermissionsQuery, Result<IEnumerable<GetPermissionsDto>>>
    {
        private readonly IUserPerissionRepsitory repository;
        public GetAllUserPermissionsHandler(IUserPerissionRepsitory repository)
        {
            this.repository = repository;
        }
        public async Task<Result<IEnumerable<GetPermissionsDto>>> Handle(GetAllUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await repository.GetUserPermissionsByUserIdAsync(request.userId);
            return Result<IEnumerable<GetPermissionsDto>>.Success(permissions);
        }
    }
}
