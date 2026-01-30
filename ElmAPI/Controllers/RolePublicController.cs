using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolePublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public RolePublicController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("GetUserRoles")]
        [ProducesResponseType(typeof(Result<IEnumerable<RoleDto>>), 200)]
        public async Task<IActionResult> GetUserRoles([FromBody] string userId)
        => HandleResult(await mediator.Send(new GetRolesByUserIdQuery(userId)));

    }
}
