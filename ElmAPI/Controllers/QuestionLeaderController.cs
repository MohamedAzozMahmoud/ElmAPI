using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Leader")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/leader/[controller]")]
    [ApiController]
    public class QuestionLeaderController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionLeaderController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/Question/ExportTemplateForQuestionsQuery/QuestionsBankId
        [HttpGet]
        [Route("ExportTemplateForQuestions/{questionsBankId:int}")]
        [ProducesResponseType(typeof(FileResult), 200)]
        public async Task<IActionResult> ExportTemplateForQuestions([FromRoute] int questionsBankId)
        {
            var result = await mediator.Send(new ExportTemplateForQuestionsQuery(questionsBankId));
            if (!result.IsSuccess && result.Data == null)
                return StatusCode(result.StatusCode, result.Message);
            var stream = result.Data;
            var fileName = $"QuestionBank_{questionsBankId}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // POST: api/Question
        [HttpPost]
        [Route("AddQuestion")]
        [ProducesResponseType(typeof(QuestionsDto), 200)]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionCommand command)
            => HandleResult(await mediator.Send(command));

        // POST : api/Question/AddRingQuestions/QuestionsBankId
        [HttpPost]
        [Route("AddRingQuestions/{questionsBankId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddRingQuestions([FromRoute] int questionsBankId, [FromBody] List<AddQuestionsDto> questionsDtos)
            => HandleResult(await mediator.Send(new AddRingQuestionsCommand(questionsBankId, questionsDtos)));

        // POST : api/Question/AddByExcelQuestions/QuestionsBankId
        [HttpPost]
        [Route("AddByExcelQuestions")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddByExcelQuestions([FromForm] AddByExcelQuestionsDto addBy)
        {
            if (addBy.File == null)
                return BadRequest("قم بتحميل ملف");
            var extension = Path.GetExtension(addBy.File.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
                return BadRequest("يجب أن يكون .xlsx أو .xls");

            using var stream = addBy.File.OpenReadStream();
            return HandleResult(await mediator.Send(new AddByExcelQuestionsCommand(addBy.QuestionBankId, stream)));
        }

        // PUT: api/Question
        [HttpPut]
        [Route("UpdateQuestion")]
        [ProducesResponseType(typeof(QuestionsDto), 200)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/Question/Id
        [HttpDelete]
        [Route("{questionId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int questionId)
            => HandleResult(await mediator.Send(new DeleteQuestionCommand(questionId)));

    }
}
