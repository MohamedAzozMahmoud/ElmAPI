using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize(Roles = "Leader")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/leader/[controller]")]
    [ApiController]
    public class QuestionsBankLeaderController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionsBankLeaderController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // Post: api/QuestionsBanks
        [HttpPost]
        [Route("CreateQuestion")]
        [ProducesResponseType(typeof(Result<QuestionsBankDto>), 200)]
        public async Task<IActionResult> CreateQuestion([FromBody] AddQuestionsBankCommand command)
            => HandleResult(await mediator.Send(command));

        // Put: api/QuestionsBanks
        [HttpPut]
        [Route("UpdateQuestion")]
        [ProducesResponseType(typeof(Result<QuestionsBankDto>), 200)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionsBankCommand command)
            => HandleResult(await mediator.Send(command));

        // Delete: api/QuestionsBanks/{id}
        [HttpDelete]
        [Route("DeleteQuestion/{id:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
            => HandleResult(await mediator.Send(new DeleteQuestionsBankCommand(id)));
    }
}
