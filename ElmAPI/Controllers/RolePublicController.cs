using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize]
    [EnableRateLimiting("UserRolePolicy")]
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
        [Route("GetUserRoles/{userId}")]
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), 200)]
        public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
        => HandleResult(await mediator.Send(new GetRolesByUserIdQuery(userId)));

    }
}
