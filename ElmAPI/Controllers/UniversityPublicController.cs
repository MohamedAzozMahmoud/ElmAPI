using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Features.University.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public UniversityPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // GET: api/University/{name}
        [HttpGet]
        [Route("GetUniversityByName/{name}")]
        [ProducesResponseType(typeof(Result<UniversityDetialsDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] string name)
            => HandleResult(await mediator.Send(new GetUniversityByNameQuery(name)));
    }
}
