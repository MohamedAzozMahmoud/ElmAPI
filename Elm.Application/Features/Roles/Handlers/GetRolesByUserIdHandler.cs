using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Roles.Queries;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{
    public sealed class GetRolesByUserIdHandler : IRequestHandler<GetRolesByUserIdQuery, Result<IEnumerable<RoleDto>>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        public GetRolesByUserIdHandler(UserManager<AppUser> _userManager, IMapper _mapper)
        {
            userManager = _userManager;
            mapper = _mapper;
        }
        public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = userManager.Users.SingleOrDefault(x => x.Id == request.userId);
            if (user == null)
            {
                return Result<IEnumerable<RoleDto>>.Failure("Not found user");
            }
            var strings = userManager.GetRolesAsync(user).Result.ToList();
            if (strings == null)
            {
                return Result<IEnumerable<RoleDto>>.Failure("Not found roles");
            }
            var roleDtos = mapper.Map<IEnumerable<RoleDto>>(strings);
            return Result<IEnumerable<RoleDto>>.Success(roleDtos);
        }
    }
}
