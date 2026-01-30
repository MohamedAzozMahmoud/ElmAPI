using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CollegeAdminController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public CollegeAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST: api/College
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(Result<CollegeDto>), 200)]
        public async Task<IActionResult> CreateCollege([FromBody] AddCollegeCommand command)
            => HandleResult(await _mediator.Send(command));


        // PUT: api/College/
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(Result<CollegeDto>), 200)]
        public async Task<IActionResult> UpdateCollege([FromBody] UpdateCollegeCommand command)
            => HandleResult(await _mediator.Send(command));

        // DELETE: api/College/Id
        [HttpDelete]
        [Route("Delete/{collegeId:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteCollege([FromRoute] int collegeId)
            => HandleResult(await _mediator.Send(new DeleteCollegeCommand(collegeId)));

    }
}
