using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("LoginPolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthAdminController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public AuthAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST: api/Auth/RegisterAdmin
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[Route("RegisterAdmin")]
        //[ProducesResponseType(typeof(Result<bool>), 200)]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterCommand command) =>
        //    HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RegisterLeader
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterLeader")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> RegisterLeader([FromBody] RegisterLeaderCommand command) =>
            HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RegisterDoctor
        [HttpPost]
        [Route("RegisterDoctor")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorCommand command) =>
            HandleResult(await _mediator.Send(command));

        // DELETE: api/Auth/Delete
        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Delete([FromBody] DeleteCommand command)
            => HandleResult(await _mediator.Send(command));

        //[HttpGet]
        //[Route("GetAllUsers/{role:alpha}")]
        //[ProducesResponseType(typeof(Result<IEnumerable<UserDto>>), 200)]
        //public async Task<IActionResult> GetAllUsers([FromRoute] string role)
        //=> HandleResult(await _mediator.Send(new GetAllUsersQuery(role)));

        [HttpGet]
        [Route("GetAllDoctor")]
        [ProducesResponseType(typeof(Result<IEnumerable<DoctorDto>>), 200)]
        public async Task<IActionResult> GetAllDoctor([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
            => HandleResult(await _mediator.Send(new GetAllDoctorsQuery(pageSize, pageNumber)));

        [HttpGet]
        [Route("GetAllLeader")]
        [ProducesResponseType(typeof(Result<IEnumerable<LeaderDto>>), 200)]
        public async Task<IActionResult> GetAllLeader([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
            => HandleResult(await _mediator.Send(new GetAllLeadersQuery(pageSize, pageNumber)));

        // PUT: api/Auth/ActivateUser
        [HttpPut]
        [Route("ActivateUser/{userId}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> ActivateUser([FromRoute] string userId)
            => HandleResult(await _mediator.Send(new ActivateUserCommand(userId)));

        // PUT: api/Auth/DeactivateUser
        [HttpPut]
        [Route("DeactivateUser/{userId}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeactivateUser([FromRoute] string userId)
            => HandleResult(await _mediator.Send(new DeactivateUserCommand(userId)));

    }
}
