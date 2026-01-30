using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // GET: api/Question/ByBank/Id
        [HttpGet]
        [Route("ByBank/{questionsBankId:int}")]
        [ProducesResponseType(typeof(Result<List<QuestionsDto>>), 200)]
        public async Task<IActionResult> GetQuestionsByBankId([FromRoute] int questionsBankId)
        => HandleResult(await mediator.Send(new GetAllQuestionsQuery(questionsBankId)));

        // GET: api/Question/Count/ByBank/Id
        [HttpGet]
        [Route("Count/ByBank/{questionsBankId:int}")]
        [ProducesResponseType(typeof(Result<int>), 200)]
        public async Task<IActionResult> GetQuestionCountByBankId([FromRoute] int questionsBankId)
            => HandleResult(await mediator.Send(new GetBankCountQuery(questionsBankId)));

        // GET: api/Question/Id
        [HttpGet]
        [Route("{questionId:int}")]
        [ProducesResponseType(typeof(Result<QuestionsDto>), 200)]
        public async Task<IActionResult> GetQuestionById([FromRoute] int questionId)
            => HandleResult(await mediator.Send(new GetQuestionByIdQuery(questionId)));

    }
}
