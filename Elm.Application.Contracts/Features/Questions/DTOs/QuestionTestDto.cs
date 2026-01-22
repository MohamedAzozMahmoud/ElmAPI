using Elm.Application.Contracts.Features.Options.DTOs;

namespace Elm.Application.Contracts.Features.Questions.DTOs
{
    public record QuestionTestDto
    {
        public int Id { get; init; }
        public string Content { get; init; } = null!;
        public string QuestionType { get; init; } = null!;
        public List<OptionTestDto> Options { get; set; } = [];
    }
}
