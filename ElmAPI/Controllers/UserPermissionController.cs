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
    public class UserPermissionController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public UserPermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/UserPermission/AddUserPermission
        [HttpPost("AddUserPermission")]
        public async Task<IActionResult> AddUserPermission([FromBody] AddUserPermissionCommand command)
            => HandleResult(await _mediator.Send(command));


        // api/UserPermission/RemoveUserPermission
        [HttpPost("RemoveUserPermission")]
        public async Task<IActionResult> RemoveUserPermission([FromBody] DeleteUserPermissionCommand command)
            => HandleResult(await _mediator.Send(command));

        // api/UserPermission/GetPermissionsByUserId
        [AllowAnonymous]
        [HttpGet("GetPermissionsByUserId/{userId}")]
        public async Task<IActionResult> GetPermissionsByUserId([FromRoute] string userId)
            => HandleResult(await _mediator.Send(new GetAllUserPermissionsQuery(userId)));

    }
}
