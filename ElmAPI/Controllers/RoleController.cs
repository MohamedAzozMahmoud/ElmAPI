using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Application.Contracts.Features.Roles.Queries;
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
    public class RoleController : ApiBaseController
    {
        private readonly IMediator mediator;

        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(AddRoleCommand command) =>
            Ok(await mediator.Send(command));

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles() =>
            Ok(await mediator.Send(new GetRolesQuery()));

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(UpdateRoleCommand command) =>
            Ok(await mediator.Send(command));

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(DeleteRoleCommand command) =>
            Ok(await mediator.Send(command));
    }
}
