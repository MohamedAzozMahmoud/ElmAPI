using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
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
    public class UserPermissionPublicController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public UserPermissionPublicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/UserPermission/GetPermissionsByUserId
        [HttpGet]
        [Route("GetPermissionsByUserId/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<GetPermissionsDto>), 200)]
        public async Task<IActionResult> GetPermissionsByUserId([FromRoute] string userId)
            => HandleResult(await _mediator.Send(new GetAllUserPermissionsQuery(userId)));

    }
}
