using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Roles.Queries;
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
    public class RoleAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public RoleAdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("CreateRole")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpGet]
        [Route("GetAllRoles")]
        [ProducesResponseType(typeof(List<RoleDto>), 200)]
        public async Task<IActionResult> GetAllRoles() =>
            HandleResult(await mediator.Send(new GetRolesQuery()));

        [HttpPut]
        [Route("UpdateRole")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command) =>
            HandleResult(await mediator.Send(command));

        [HttpDelete]
        [Route("DeleteRole")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand command) =>
            HandleResult(await mediator.Send(command));
    }
}
