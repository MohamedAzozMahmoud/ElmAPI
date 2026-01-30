using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public SubjectPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // GET: api/Subject/departments
        [HttpGet]
        [Route("GetAllSubjects/{departmentId:int}")]
        [ProducesResponseType(typeof(Result<List<GetSubjectDto>>), 200)]
        public async Task<IActionResult> GetAllSubjects([FromRoute] int departmentId)
                 => HandleResult(await mediator.Send(new GetAllSubjectQuery(departmentId)));

        // Get: api/Subject/Id
        [HttpGet]
        [Route("GetSubjectById/{id:int}")]
        [ProducesResponseType(typeof(Result<GetSubjectDto>), 200)]
        public async Task<IActionResult> GetSubjectById([FromRoute] int id)
                 => HandleResult(await mediator.Send(new GetSubjectByIdQuery(id)));

    }
}
