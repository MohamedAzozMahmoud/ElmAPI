using Microsoft.AspNetCore.Http;

namespace Elm.Application.Contracts.Features.Questions.DTOs
{
    public record AddByExcelQuestionsDto
    {
        public IFormFile File { get; set; }
        public int QuestionBankId { get; set; }
    }
}
