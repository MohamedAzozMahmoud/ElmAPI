namespace Elm.Application.Contracts.Features.Test.DTOs
{
    // ✅ بيانات الامتحان المُخزنة في الـ Cache
    public record CachedTestSession
    {
        public int QuestionBankId { get; init; }
        public DateTime ExpiresAt { get; init; }
        public Dictionary<int, HashSet<int>> CorrectOptionsByQuestionId { get; init; } = new();
        // Key: QuestionId, Value: HashSet of correct OptionIds
    }
}
