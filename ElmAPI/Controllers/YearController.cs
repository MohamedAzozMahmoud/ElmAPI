using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.Queries;
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
    public class YearController : ApiBaseController
    {
        private readonly IMediator mediator;

        public YearController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<YearController>
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet]
        [Route("GetAllYears/{collegeId:int}")]
        public async Task<IActionResult> GetAllYears([FromQuery] int collegeId)
        => Ok(await mediator.Send(new GetAllYearQuery(collegeId)));

        // GET api/<YearController>/Id
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet]
        [Route("GetYearById/{yearId:int}")]
        public async Task<IActionResult> GetYearById([FromRoute] int yearId)
            => Ok(await mediator.Send(new GetYearByIdQuery(yearId)));

        // POST api/<YearController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddYearCommand command)
            => Ok(await mediator.Send(command));

        // PUT api/<YearController>/Id
        [HttpPut]
        [Route("UpdateYear")]
        public async Task<IActionResult> Put([FromBody] UpdateYearCommand command)
            => Ok(await mediator.Send(command));

        // DELETE api/<YearController>/Id
        [HttpDelete]
        [Route("DeleteYear/{yearId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int yearId)
            => Ok(await mediator.Send(new DeleteYearCommand(yearId)));

    }
}
