using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionPublicController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public RolePermissionPublicController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // api/RolePermission/GetPermissionsByRoleName
        [HttpGet]
        [Route("GetPermissionsByRoleName/{roleName}")]
        [ProducesResponseType(typeof(Result<List<GetPermissionsDto>>), 200)]
        public async Task<IActionResult> GetPermissionsByRoleName([FromRoute] string roleName)
            => HandleResult(await _mediator.Send(new GetAllRolePermissionsQuery(roleName)));
    }
}
