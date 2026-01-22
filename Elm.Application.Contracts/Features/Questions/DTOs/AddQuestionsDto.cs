using Elm.Application.Contracts.Features.Options.DTOs;

namespace Elm.Application.Contracts.Features.Questions.DTOs
{
    public record AddQuestionsDto
    {
        public string Content { get; set; }
        public string QuestionType { get; set; }
        public IEnumerable<AddOptionsDto> Options { get; set; }
    }
}
