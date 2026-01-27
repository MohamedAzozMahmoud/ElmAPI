using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.DTOs;
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
    public class UniversityAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public UniversityAdminController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/University
        [HttpPost]
        [Route("AddUniversity")]
        [ProducesResponseType(typeof(UniversityDto), 200)]
        public async Task<IActionResult> Post([FromBody] AddUniversityCommand command)
            => HandleResult(await mediator.Send(command));

        // PUT: api/University
        [HttpPut]
        [Route("UpdateUniversity")]
        [ProducesResponseType(typeof(UniversityDto), 200)]
        public async Task<IActionResult> Put([FromBody] UpdateUniversityCommand command)
            => HandleResult(await mediator.Send(command));
        // DELETE: api/University
        [HttpDelete]
        [Route("DeleteUniversity/{id:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => HandleResult(await mediator.Send(new DeleteUniversityCommand(id)));

    }
}
