using Elm.Application.Contracts.Features.Test.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ApiBaseController
    {
        private readonly IMediator mediator;

        public TestController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // post: api/test/start
        [HttpPost("start")]
        public async Task<IActionResult> StartTest([FromBody] StartTestCommand command)
        => HandleResult(await mediator.Send(command));


        // post: api/test/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitTest([FromBody] SubmitTestCommand command)
            => HandleResult(await mediator.Send(command));

    }
}
