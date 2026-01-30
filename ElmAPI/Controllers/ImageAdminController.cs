using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Images.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ImageAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public ImageAdminController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // POST: api/Image
        [HttpPost]
        [Route("UploadCollegeImage")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UploadCollegeImage([FromForm] AddCollegeImageCommand command)
        => HandleResult(await mediator.Send(command));

        // POST: api/Image
        [HttpPost]
        [Route("UploadUniversityImage")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UploadUniversityImage([FromForm] AddUniversityImageCommand command)
        => HandleResult(await mediator.Send(command));

        // DELETE: api/Image
        [HttpDelete]
        [Route("DeleteCollegeImage")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteCollegeImage([FromQuery] DeleteCollegeImageCommand command)
        => HandleResult(await mediator.Send(command));

        // DELETE: api/Image
        [HttpDelete]
        [Route("DeleteUniversityImage")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteUniversityImage([FromQuery] DeleteUniversityImageCommand command)
            => HandleResult(await mediator.Send(command));
    }
}
