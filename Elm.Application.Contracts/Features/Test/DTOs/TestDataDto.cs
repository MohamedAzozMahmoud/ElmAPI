using Elm.Application.Contracts.Features.Questions.DTOs;

namespace Elm.Application.Contracts.Features.Test.DTOs
{
    public record TestDataDto
    {
        public Guid TestSessionId { get; init; }  // ✅ معرف فريد لكل امتحان
        public int QuestionBankId { get; init; }  // ✅ للتتبع
        public TimeSpan Duration { get; init; }
        public DateTime ExpiresAt { get; init; }  // ✅ وقت الانتهاء
        public IEnumerable<QuestionTestDto> Questions { get; init; } = [];
    }
}
