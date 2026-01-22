using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize(Roles = "Leader")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsBankController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionsBankController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // Get: api/QuestionsBanks by CurriculumId
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("QuestionsBanks/{curriculumId:int}")]
        public async Task<IActionResult> GetAllQuestionsBanks(int curriculumId)
            => HandleResult(await mediator.Send(new GetAllQuestionsBankQuery(curriculumId)));

        // Get: api/QuestionsBanks/{id}
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuestionById(int id)
            => HandleResult(await mediator.Send(new GetQuestionsBankByIdQuery(id)));

        // Post: api/QuestionsBanks
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] AddQuestionsBankCommand command)
            => HandleResult(await mediator.Send(command));

        // Put: api/QuestionsBanks
        [HttpPut]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionsBankCommand command)
            => HandleResult(await mediator.Send(command));

        // Delete: api/QuestionsBanks/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteQuestion(int id)
            => HandleResult(await mediator.Send(new DeleteQuestionsBankCommand(id)));

    }
}
