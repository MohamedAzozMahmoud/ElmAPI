using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Roles.Queries;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{

    public sealed class GetRolesHandler : IRequestHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
    {
        private readonly RoleManager<Role> roleManager;
        public GetRolesHandler(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = roleManager.Roles.ToList();
            var roleDtos = roles.Select(r => new RoleDto { Name = r.Name }).ToList();
            return Result<IEnumerable<RoleDto>>.Success(roleDtos);
        }
    }
}
