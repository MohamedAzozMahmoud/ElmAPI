using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Authentication.Handlers
{
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly UserManager<AppUser> userManage;
        private readonly IMapper mapper;

        public GetAllUsersHandler(UserManager<AppUser> _userManage, IMapper _mapper)
        {
            userManage = _userManage;
            mapper = _mapper;
        }
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var found = UserRoles.IsValidRole(request.role);
            if (!found)
            {
                return Result<IEnumerable<UserDto>>.Failure("Invalid role specified.", 400);
            }
            var users = await userManage.GetUsersInRoleAsync(request.role);
            var usersMap = mapper.Map<IEnumerable<UserDto>>(users);
            return Result<IEnumerable<UserDto>>.Success(usersMap);
        }
    }
}
