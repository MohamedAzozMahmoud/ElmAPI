namespace Elm.Application.Contracts.Features.Test.DTOs
{
    public record SubmittedAnswerDto
    {
        public int QuestionId { get; init; }
        public List<int> SelectedOptionIds { get; init; } = [];  // ✅ IDs مش Content
    }
}
