using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class YearPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public YearPublicController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<YearController>
        [HttpGet]
        [Route("GetAllYears/{collegeId:int}")]
        [ProducesResponseType(typeof(List<GetYearDto>), 200)]
        public async Task<IActionResult> GetAllYears([FromQuery] int collegeId)
        => HandleResult(await mediator.Send(new GetAllYearQuery(collegeId)));

        // GET api/<YearController>/Id
        [HttpGet]
        [Route("GetYearById/{yearId:int}")]
        [ProducesResponseType(typeof(GetYearDto), 200)]
        public async Task<IActionResult> GetYearById([FromRoute] int yearId)
            => Ok(await mediator.Send(new GetYearByIdQuery(yearId)));
    }
}
