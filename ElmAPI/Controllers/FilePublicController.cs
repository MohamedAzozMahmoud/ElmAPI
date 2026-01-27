using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Files.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FilePublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public FilePublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/File/DownloadFile/{fileName}
        [HttpGet]
        [Route("DownloadFile/{fileName}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
        {
            var result = await mediator.Send(new DownloadFileCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);

            return File(result.Data.Content, result.Data.ContentType, result.Data.ContentType);
        }

        // GET: api/File/ShowFileFromUrl/{fileName}
        [HttpGet]
        [Route("ShowFileFromUrl/{fileName}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        public async Task<IActionResult> ShowFileFromUrl([FromRoute] string fileName)
        {
            var result = await mediator.Send(new ViewFileCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);
            // Return the file as a FileStreamResult
            return File(result.Data.Content, result.Data.ContentType);
        }

        // GET: api/File/GetAllFilesByCurriculumId/{curriculumId} 
        [HttpGet]
        [Route("GetAllFilesByCurriculumId/{curriculumId:int}")]
        [ProducesResponseType(typeof(List<FileView>), 200)]
        public async Task<IActionResult> GetAllFilesByCurriculumId([FromRoute] int curriculumId)
            => HandleResult(await mediator.Send(new GetAllFilesByCurriculumIdQuery(curriculumId)));

    }
}
