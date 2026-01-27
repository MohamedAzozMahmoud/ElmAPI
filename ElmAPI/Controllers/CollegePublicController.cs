using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.College.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CollegePublicController : ApiBaseController
    {
        private readonly IMediator _mediator;
        public CollegePublicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/College
        [HttpGet]
        [Route("GetAllColleges/{universityId:int}")]
        [ProducesResponseType(typeof(List<GetCollegeDto>), 200)]
        public async Task<IActionResult> GetAllColleges([FromRoute] int universityId)
            => HandleResult(await _mediator.Send(new GetAllCollegesQuery(universityId)));

        // GET: api/College/Id
        [HttpGet]
        [Route("GetCollegeById/{collegeId:int}")]
        [ProducesResponseType(typeof(GetCollegeDto), 200)]
        public async Task<IActionResult> GetCollegeById([FromRoute] int collegeId)
            => HandleResult(await _mediator.Send(new GetCollegeByIdQuery(collegeId)));


    }
}
