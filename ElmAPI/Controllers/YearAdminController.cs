using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.DTOs;
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
    public class YearAdminController : ApiBaseController
    {
        private readonly IMediator mediator;
        public YearAdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // POST api/<YearAdminController>
        [HttpPost]
        [Route("AddYear")]
        [ProducesResponseType(typeof(YearDto), 200)]
        public async Task<IActionResult> Post([FromBody] AddYearCommand command)
            => HandleResult(await mediator.Send(command));

        // PUT api/<YearAdminController>/Id
        [HttpPut]
        [Route("UpdateYear")]
        [ProducesResponseType(typeof(YearDto), 200)]
        public async Task<IActionResult> Put([FromBody] UpdateYearCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE api/<YearAdminController>/Id
        [HttpDelete]
        [Route("DeleteYear/{yearId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete([FromRoute] int yearId)
            => HandleResult(await mediator.Send(new DeleteYearCommand(yearId)));

    }
}
