using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsBankPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionsBankPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // Get: api/QuestionsBanks by CurriculumId
        [HttpGet]
        [Route("QuestionsBanks/{curriculumId:int}")]
        [ProducesResponseType(typeof(Result<List<QuestionsBankDto>>), 200)]
        public async Task<IActionResult> GetAllQuestionsBanks([FromRoute] int curriculumId)
            => HandleResult(await mediator.Send(new GetAllQuestionsBankQuery(curriculumId)));

        // Get: api/QuestionsBanks/{id}
        [HttpGet]
        [Route("QuestionsBankById/{id:int}")]
        [ProducesResponseType(typeof(Result<QuestionsBankDto>), 200)]
        public async Task<IActionResult> GetQuestionById(int id)
            => HandleResult(await mediator.Send(new GetQuestionsBankByIdQuery(id)));

    }
}
