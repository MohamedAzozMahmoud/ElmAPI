using Elm.Application.Contracts;
using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using Elm.Application.Mapper.Elm.Application.Mappers;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly UserManager<AppUser> userManage;
        private readonly MappingProvider _mapping;

        public GetAllUsersHandler(UserManager<AppUser> _userManage, MappingProvider mapping)
        {
            userManage = _userManage;
            _mapping = mapping;
        }
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var found = UserRoles.IsValidRole(request.role);
            if (!found)
            {
                return Result<IEnumerable<UserDto>>.Failure("Invalid role specified.", 400);
            }
            var users = await userManage.GetUsersInRoleAsync(request.role);
            var usersMap = _mapping.MapToDtoList(users.ToList());
            return Result<IEnumerable<UserDto>>.Success(usersMap);
        }
    }
}
