using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Features.Options.DTOs;
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
    public class OptionController : ApiBaseController
    {
        private readonly IMediator mediator;

        public OptionController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // POST: api/Option
        [HttpPost]
        [Route("AddOption")]
        [ProducesResponseType(typeof(OptionsDto), 200)]
        public async Task<IActionResult> Post([FromBody] AddOptionCommand command)
        => HandleResult(await mediator.Send(command));

        // PUT: api/Option
        [HttpPut]
        [Route("UpdateOption")]
        [ProducesResponseType(typeof(OptionsDto), 200)]
        public async Task<IActionResult> Put([FromBody] UpdateOptionCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/Option/id
        [HttpDelete]
        [Route("DeleteOption/{optionId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete([FromRoute] int optionId)
            => HandleResult(await mediator.Send(new DeleteOptionCommand(optionId)));
    }
}
