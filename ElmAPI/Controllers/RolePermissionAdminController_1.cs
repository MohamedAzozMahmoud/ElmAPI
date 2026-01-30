using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
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
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> AddRolePermission([FromBody] AddRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));


        // api/RolePermission/RemoveRolePermission
        [HttpPost]
        [Route("RemoveRolePermission")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> RemoveRolePermission([FromBody] DeleteRolePermissionCommand command)
            => HandleResult(await _mediator.Send(command));

    }
}
