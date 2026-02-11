using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Images.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [Route("DeleteImage/{fileName}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteImage([FromRoute] string fileName)
            => HandleResult(await mediator.Send(new DeleteImageByNameCommand(fileName)));


    }
}
