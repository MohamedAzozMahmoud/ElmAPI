using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.Queries;
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
    public class UniversityController : ApiBaseController
    {
        private readonly IMediator mediator;

        public UniversityController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/University
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUniversityCommand command)
            => HandleResult(await mediator.Send(command));

        // PUT: api/University
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUniversityCommand command)
            => HandleResult(await mediator.Send(command));
        // DELETE: api/University
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => HandleResult(await mediator.Send(new DeleteUniversityCommand(id)));

        // GET: api/University/{id}
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => HandleResult(await mediator.Send(new GetUniversityByIdQuery(id)));
    }
}
