using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Contracts.Features.Files.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiBaseController
    {
        private readonly IMediator mediator;

        public FileController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // POST: api/File
        [Authorize(Roles = "Leader")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] UploadFileCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/File/{id}
        [Authorize(Roles = "Leader")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => HandleResult(await mediator.Send(new DeleteFileCommand(id)));

        // GET: api/File/DownloadFile/{fileName}
        [AllowAnonymous]
        [HttpGet("DownloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var result = await mediator.Send(new DownloadFileCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);

            return File(result.Data.Content, result.Data.ContentType, result.Data.ContentType);
        }

        // GET: api/File/ShowFileFromUrl/{fileName}
        [AllowAnonymous]
        [HttpGet("ShowFileFromUrl/{fileName}")]
        public async Task<IActionResult> ShowFileFromUrl(string fileName)
        {
            var result = await mediator.Send(new ViewFileCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);
            // Return the file as a FileStreamResult
            return File(result.Data.Content, result.Data.ContentType);
        }

        // GET: api/File/GetAllFilesByCurriculumId/{curriculumId}
        [AllowAnonymous]
        [HttpGet("GetAllFilesByCurriculumId/{curriculumId:int}")]
        public async Task<IActionResult> GetAllFilesByCurriculumId(int curriculumId)
            => HandleResult(await mediator.Send(new GetAllFilesByCurriculumIdQuery(curriculumId)));

        // POST: api/File/RateFile
        [Authorize(Roles = "Doctor")]
        [HttpPost("RateFile")]
        public async Task<IActionResult> RateFile([FromBody] RatingFileCommand command)
            => HandleResult(await mediator.Send(command));


    }
}
