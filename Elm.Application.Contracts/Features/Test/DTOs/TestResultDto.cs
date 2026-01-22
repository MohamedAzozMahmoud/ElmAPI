namespace Elm.Application.Contracts.Features.Test.DTOs
{
    // ✅ نتيجة مفصلة
    public record TestResultDto
    {
        public int TotalQuestions { get; init; }
        public int CorrectAnswers { get; init; }
        public double ScorePercentage { get; init; }
    }
}
