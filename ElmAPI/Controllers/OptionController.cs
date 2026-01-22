using Elm.Application.Contracts.Features.Options.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
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
        public async Task<IActionResult> Post([FromBody] AddOptionCommand command)
        => HandleResult(await mediator.Send(command));

        // PUT: api/Option
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateOptionCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/Option/id
        [HttpDelete("{optionId:int}")]
        public async Task<IActionResult> Delete(int optionId)
            => HandleResult(await mediator.Send(new DeleteOptionCommand(optionId)));
    }
}
