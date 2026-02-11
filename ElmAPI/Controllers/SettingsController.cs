using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ApiBaseController
    {
        private readonly IMediator mediator;

        public SettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // GET: api/Settings
        [HttpGet]
        [Route("GetAllSettings")]
        [ProducesResponseType(typeof(Result<List<SettingsDto>>), 200)]
        public async Task<IActionResult> Get()
        => HandleResult(await mediator.Send(new GetAllSettingsQuery()));

        // Put: api/Settings
        [HttpPut]
        [Route("UpdateSettings")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsCommand command)
            => HandleResult(await mediator.Send(command));

        // POST: api/Settings
        [HttpPost]
        [Route("CreateSetting")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> CreateSetting([FromBody] AddSettingsCommand command)
            => HandleResult(await mediator.Send(command));

    }
}
