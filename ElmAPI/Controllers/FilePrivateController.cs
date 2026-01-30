using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Files.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/private/[controller]")]
    [ApiController]
    public class FilePrivateController : ApiBaseController
    {
        private readonly IMediator mediator;

        public FilePrivateController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/File
        //[Authorize(Roles = "Leader")]
        [HttpPost]
        [Route("UploadFile")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Result<string>), 200)]
        public async Task<IActionResult> Post([FromForm] UploadFileCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/File/{id}
        [Authorize(Roles = "Leader")]
        [HttpDelete]
        [Route("DeleteFile/{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> Delete([FromRoute] int id)
            => HandleResult(await mediator.Send(new DeleteFileCommand(id)));
        // POST: api/File/RateFile
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        [Route("RateFile")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> RateFile([FromBody] RatingFileCommand command)
            => HandleResult(await mediator.Send(command));

    }
}
