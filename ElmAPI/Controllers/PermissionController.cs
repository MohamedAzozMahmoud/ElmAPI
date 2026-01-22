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
    public class PermissionController : ApiBaseController
    {
        private readonly IMediator mediator;

        public PermissionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpPut("UpdatePermission")]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermissionCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpGet("GetAllPermissions")]
        public async Task<IActionResult> GetAllPermissions() =>
            HandleResult(await mediator.Send(new GetAllPermissionsQuery()));

        [HttpDelete("DeletePermission")]
        public async Task<IActionResult> DeletePermission([FromBody] DeletePermissionCommand command) =>
            HandleResult(await mediator.Send(command));
    }
}
