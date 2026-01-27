using Elm.Application.Contracts.Features.Permissions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Admin")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserPermissionAdminController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public UserPermissionAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/UserPermission/AddUserPermission
        [HttpPost]
        [Route("AddUserPermission")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddUserPermission([FromBody] AddUserPermissionCommand command)
            => HandleResult(await _mediator.Send(command));


        // api/UserPermission/RemoveUserPermission
        [HttpPost]
        [Route("RemoveUserPermission")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RemoveUserPermission([FromBody] DeleteUserPermissionCommand command)
            => HandleResult(await _mediator.Send(command));

    }
}
