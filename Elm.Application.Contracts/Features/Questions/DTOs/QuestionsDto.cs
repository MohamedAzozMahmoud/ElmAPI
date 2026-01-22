using Elm.Application.Contracts.Features.Options.DTOs;

namespace Elm.Application.Contracts.Features.Questions.DTOs
{
    public record QuestionsDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string QuestionType { get; set; }
        public IEnumerable<OptionsDto> Options { get; set; }
    }
}
