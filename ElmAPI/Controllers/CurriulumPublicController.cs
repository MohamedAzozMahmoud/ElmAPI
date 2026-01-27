using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CurriulumPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public CurriulumPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }


        // Get: api/CurriculumControllers
        //GetAllCurriculumsByDeptIdAndYearIdAsync
        [HttpGet]
        [Route("GetAllByDeptIdAndYearId/{departmentId:int}")]
        [ProducesResponseType(typeof(List<GetCurriculumDto>), 200)]
        public async Task<IActionResult> GetAllByDeptIdAndYearIdAsync([FromRoute] int departmentId, [FromQuery] int yearId)
        => HandleResult(await mediator.Send(new GetAllCurriculumQuery(departmentId, yearId)));

        // Get: api/CurriculumControllers/Id
        [HttpGet]
        [Route("GetById/{id:int}")]
        [ProducesResponseType(typeof(GetCurriculumDto), 200)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
            => HandleResult(await mediator.Send(new GetCurriculumByIdQuery(id)));


    }
}
