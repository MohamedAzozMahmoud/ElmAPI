using Elm.Application.Contracts.Features.Test.Commands;
using Elm.Application.Contracts.Features.Test.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
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
        [HttpPost]
        [Route("start")]
        [ProducesResponseType(typeof(TestDataDto), 200)]
        public async Task<IActionResult> StartTest([FromBody] StartTestCommand command)
        => HandleResult(await mediator.Send(command));


        // post: api/test/submit
        [HttpPost]
        [Route("submit")]
        [ProducesResponseType(typeof(TestResultDto), 200)]
        public async Task<IActionResult> SubmitTest([FromBody] SubmitTestCommand command)
            => HandleResult(await mediator.Send(command));

    }
}
