using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class SubjectAdminController : ApiBaseController
    {
        private readonly IMediator mediator;
        public SubjectAdminController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/Subject
        [HttpPost]
        [Route("AddSubject")]
        [ProducesResponseType(typeof(Result<SubjectDto>), 200)]
        public async Task<IActionResult> AddSubject([FromBody] AddSubjectCommand command)
                 => HandleResult(await mediator.Send(command));

        // PUT: api/Subject
        [HttpPut]
        [Route("UpdateSubject")]
        [ProducesResponseType(typeof(Result<SubjectDto>), 200)]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectCommand command)
                 => HandleResult(await mediator.Send(command));

        // DELETE: api/Subject/Id
        [HttpDelete]
        [Route("DeleteSubject/{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteSubject([FromRoute] int id)
                 => HandleResult(await mediator.Send(new DeleteSubjectCommand(id)));

    }
}
