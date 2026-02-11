using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Authorize(Roles = "Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CurriulumAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public CurriulumAdminController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // Post: api/CurriculumControllers
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(Result<CurriculumDto>), 200)]
        public async Task<IActionResult> CreateAsync([FromBody] AddCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Put: api/CurriculumControllers/Id
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(Result<CurriculumDto>), 200)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Delete: api/CurriculumControllers/Id
        [HttpDelete]
        [Route("Delete/{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
            => HandleResult(await mediator.Send(new DeleteCurriculumCommand(id)));

        // Get: api/CurriculumControllers/BySubjectId
        [HttpGet]
        [Route("BySubjectId/{subjectId:int}")]
        [ProducesResponseType(typeof(Result<List<AdminCurriculumDto>>), 200)]
        public async Task<IActionResult> GetBySubjectIdAsync([FromRoute] int subjectId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
            => HandleResult(await mediator.Send(new GetAllCurriculumForAdminQuery(subjectId, pageSize, pageNumber)));

        // Put: api/CurriculumControllers/TogglePublish/Id
        [HttpPut]
        [Route("TogglePublish/{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> TogglePublishAsync([FromRoute] int id)
            => HandleResult(await mediator.Send(new PublishedCurriculumCommand(id)));

        // Put: api/CurriculumControllers/UpdateDate/Id
        [HttpPut]
        [Route("UpdateDate")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UpdateDateAsync([FromBody] UpdateDateCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

    }
}
