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
    public class RolePermissionAdminController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public RolePermissionAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/RolePermission/AddRolePermission
        [HttpPost]
        [Route("AddRolePermission")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddRolePermission([FromBody] AddRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));


        // api/RolePermission/RemoveRolePermission
        [HttpPost]
        [Route("RemoveRolePermission")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RemoveRolePermission([FromBody] DeleteRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));

    }
}
