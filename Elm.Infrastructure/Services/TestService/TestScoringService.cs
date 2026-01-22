using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Test.DTOs;

namespace Elm.Infrastructure.Services.TestService
{
    public class TestScoringService : ITestScoringService
    {
        public TestResultDto CalculateScore(
            Dictionary<int, HashSet<int>> correctAnswers,
            List<SubmittedAnswerDto> submittedAnswers)
        {
            int correctCount = 0;
            int totalQuestions = correctAnswers.Count;

            foreach (var answer in submittedAnswers)
            {
                if (!correctAnswers.TryGetValue(answer.QuestionId, out var correctOptionIds))
                    continue;

                var selectedIds = answer.SelectedOptionIds.ToHashSet();

                if (selectedIds.SetEquals(correctOptionIds))
                    correctCount++;
            }

            return new TestResultDto
            {
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctCount,
                ScorePercentage = Math.Round((double)correctCount / totalQuestions * 100, 2)
            };
        }
    }
}
