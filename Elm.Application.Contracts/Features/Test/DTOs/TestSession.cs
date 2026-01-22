namespace Elm.Application.Contracts.Features.Test.DTOs
{
    public record TestSession(
        Guid Id,
        int QuestionBankId,
        DateTime ExpiresAt,
        Dictionary<int, HashSet<int>> CorrectOptionsByQuestionId)
    {
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    }
}
