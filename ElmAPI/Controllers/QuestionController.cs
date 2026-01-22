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
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ApiBaseController
    {
        private readonly IMediator mediator;

        public QuestionController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // GET: api/Question/ByBank/Id
        [AllowAnonymous]
        [HttpGet("ByBank/{questionsBankId:int}")]
        public async Task<IActionResult> GetQuestionsByBankId(int questionsBankId)
        => HandleResult(await mediator.Send(new GetAllQuestionsQuery(questionsBankId)));

        // GET: api/Question/Id
        [AllowAnonymous]
        [HttpGet("{questionId:int}")]
        public async Task<IActionResult> GetQuestionById(int questionId)
            => HandleResult(await mediator.Send(new GetQuestionByIdQuery(questionId)));

        // GET: api/Question/ExportTemplateForQuestionsQuery/QuestionsBankId
        [HttpGet("ExportTemplateForQuestions/{questionsBankId:int}")]
        public async Task<IActionResult> ExportTemplateForQuestions(int questionsBankId)
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
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionCommand command)
            => HandleResult(await mediator.Send(command));

        // Post : api/Question/AddRingQuestions/QuestionsBankId
        [HttpPost("AddRingQuestions/{questionsBankId:int}")]
        public async Task<IActionResult> AddRingQuestions(int questionsBankId, [FromBody] List<AddQuestionsDto> questionsDtos)
            => HandleResult(await mediator.Send(new AddRingQuestionsCommand(questionsBankId, questionsDtos)));

        // Post : api/Question/AddByExcelQuestions/QuestionsBankId
        [HttpPost("AddByExcelQuestions")]
        [Consumes("multipart/form-data")]
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
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommand command)
            => HandleResult(await mediator.Send(command));

        // DELETE: api/Question/Id
        [HttpDelete("{questionId:int}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
            => HandleResult(await mediator.Send(new DeleteQuestionCommand(questionId)));

    }
}
