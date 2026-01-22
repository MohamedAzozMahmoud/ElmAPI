using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Admin")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public RolePermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/RolePermission/AddRolePermission
        [HttpPost("AddRolePermission")]
        public async Task<IActionResult> AddRolePermission([FromBody] AddRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));


        // api/RolePermission/RemoveRolePermission
        [HttpPost("RemoveRolePermission")]
        public async Task<IActionResult> RemoveRolePermission([FromBody] DeleteRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));

        // api/RolePermission/GetPermissionsByRoleId
        [Authorize]
        [DisableRateLimiting]
        [HttpGet("GetPermissionsByRoleId/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId([FromRoute] string roleId)
            => HandleResult(await _mediator.Send(new GetAllRolePermissionsQuery(roleId)));
    }
}
