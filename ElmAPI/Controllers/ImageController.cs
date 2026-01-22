using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Features.Images.Queries;
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
    public class ImageController : ApiBaseController
    {
        private readonly IMediator mediator;

        public ImageController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/Image
        [HttpPost("UploadCollegeImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadCollegeImage([FromForm] AddCollegeImageCommand command)
        => HandleResult(await mediator.Send(command));

        // POST: api/Image
        [HttpPost("UploadUniversityImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadUniversityImage([FromForm] AddUniversityImageCommand command)
        => HandleResult(await mediator.Send(command));

        // DELETE: api/Image
        [HttpDelete("DeleteCollegeImage")]
        public async Task<IActionResult> DeleteCollegeImage([FromQuery] DeleteCollegeImageCommand command)
        => HandleResult(await mediator.Send(command));

        // DELETE: api/Image
        [HttpDelete("DeleteUniversityImage")]
        public async Task<IActionResult> DeleteUniversityImage([FromQuery] DeleteUniversityImageCommand command)
            => HandleResult(await mediator.Send(command));

        // GET: api/Image
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("ShowImageFromUrl/{fileName}")]
        public async Task<IActionResult> ShowImageFromUrl(string fileName)
        {
            var result = await mediator.Send(new showImageFromUrlCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);

            // ارجاع الملف كـ FileStreamResult
            return File(result.Data.Content, result.Data.ContentType);
        }

    }
}
