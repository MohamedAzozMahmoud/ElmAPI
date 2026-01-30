using Elm.Application.Contracts.Features.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagePublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public ImagePublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/Image
        [HttpGet]
        [Route("ShowImageFromUrl/{fileName}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [Produces("image/jpeg", "image/png", "application/octet-stream")]
        public async Task<IActionResult> ShowImageFromUrl([FromRoute] string fileName)
        {
            var result = await mediator.Send(new showImageFromUrlCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);

            // ارجاع الملف كـ FileResult
            return File(result.Data.Content, result.Data.ContentType);
        }
    }
}
