using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
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
        [ProducesResponseType(typeof(CurriculumDto), 200)]
        public async Task<IActionResult> CreateAsync([FromBody] AddCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Put: api/CurriculumControllers/Id
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(CurriculumDto), 200)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCurriculumCommand command)
            => HandleResult(await mediator.Send(command));

        // Delete: api/CurriculumControllers/Id
        [HttpDelete]
        [Route("Delete/{id:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
            => HandleResult(await mediator.Send(new DeleteCurriculumCommand(id)));

    }
}
