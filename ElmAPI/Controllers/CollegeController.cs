using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public CollegeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/College
        [HttpGet("{universityId:int}")]
        public async Task<IActionResult> GetAllColleges(int universityId)
            => HandleResult(await _mediator.Send(new GetAllCollegeQuery(universityId)));

        // GET: api/College/Id
        [HttpGet("GetCollegeById/{collegeId:int}")]
        public async Task<IActionResult> GetCollegeById(int collegeId)
            => HandleResult(await _mediator.Send(new GetCollegeByIdQuery(collegeId)));

        // POST: api/College
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateCollege([FromBody] AddCollegeCommand command)
            => HandleResult(await _mediator.Send(command));


        // PUT: api/College/
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateCollege([FromBody] UpdateCollegeCommand command)
            => HandleResult(await _mediator.Send(command));

        // DELETE: api/College/Id
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpDelete("{collegeId:int}")]
        public async Task<IActionResult> DeleteCollege(int collegeId)
            => HandleResult(await _mediator.Send(new DeleteCollegeCommand(collegeId)));

    }
}
