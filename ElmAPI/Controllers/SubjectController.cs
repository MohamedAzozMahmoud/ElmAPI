using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Admin")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ApiBaseController
    {
        private readonly IMediator mediator;

        public SubjectController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // GET: api/Subject/departments
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("subjects/{departmentId:int}")]
        public async Task<IActionResult> GetAllSubjects(int departmentId)
                 => HandleResult(await mediator.Send(new GetAllSubjectQuery(departmentId)));

        // Get: api/Subject/Id
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSubjectById(int id)
                 => HandleResult(await mediator.Send(new GetSubjectByIdQuery(id)));

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> AddSubject([FromBody] AddSubjectCommand command)
                 => HandleResult(await mediator.Send(command));

        // PUT: api/Subject
        [HttpPut]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectCommand command)
                 => HandleResult(await mediator.Send(command));

        // DELETE: api/Subject/Id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubject(int id)
                 => HandleResult(await mediator.Send(new DeleteSubjectCommand(id)));


    }
}
