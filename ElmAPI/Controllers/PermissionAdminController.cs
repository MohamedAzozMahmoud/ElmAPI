using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class PermissionAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public PermissionAdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("AddPermission")]
        [ProducesResponseType(typeof(Result<PermissionDto>), 200)]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpPut]
        [Route("UpdatePermission")]
        [ProducesResponseType(typeof(Result<PermissionDto>), 200)]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermissionCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpGet]
        [Route("GetAllPermissions")]
        [ProducesResponseType(typeof(Result<IEnumerable<PermissionDto>>), 200)]
        public async Task<IActionResult> GetAllPermissions() =>
            HandleResult(await mediator.Send(new GetAllPermissionsQuery()));

        [HttpDelete]
        [Route("DeletePermission/{Id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeletePermission([FromRoute] int Id) =>
            HandleResult(await mediator.Send(new DeletePermissionCommand(Id)));
    }
}
