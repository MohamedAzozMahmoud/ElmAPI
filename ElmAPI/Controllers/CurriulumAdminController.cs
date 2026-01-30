using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
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

    }
}
