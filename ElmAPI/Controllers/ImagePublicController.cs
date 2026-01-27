using Elm.Application.Contracts.Features.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
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
        public async Task<IActionResult> ShowImageFromUrl([FromRoute] string fileName)
        {
            var result = await mediator.Send(new showImageFromUrlCommand(fileName));
            if (!result.IsSuccess || result.Data == null) return HandleResult(result);

            // ارجاع الملف كـ FileStreamResult
            return File(result.Data.Content, result.Data.ContentType);
        }
    }
}
