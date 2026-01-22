using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriulumController : ApiBaseController
    {
        private readonly IMediator mediator;

        public CurriulumController(IMediator _mediator)
        {
            mediator = _mediator;
        }


        // Get: api/CurriculumControllers
        //GetAllCurriculumsByDeptIdAndYearIdAsync
        [HttpGet]
        public async Task<IActionResult> GetAllByDeptIdAndYearIdAsync([FromQuery] int departmentId, [FromQuery] int yearId)
        => HandleResult(await mediator.Send(new GetAllCurriculumQuery(departmentId, yearId)));

        // Get: api/CurriculumControllers/Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
            => HandleResult(await mediator.Send(new GetCurriculumByIdQuery(id)));

        // Post: api/CurriculumControllers
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Put: api/CurriculumControllers/Id
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Delete: api/CurriculumControllers/Id
        [Authorize("Admin")]
        [EnableRateLimiting("UserRolePolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
            => HandleResult(await mediator.Send(new DeleteCurriculumCommand(id)));

    }
}
