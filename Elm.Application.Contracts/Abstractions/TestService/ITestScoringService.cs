using Elm.Application.Contracts.Features.Test.DTOs;

namespace Elm.Application.Contracts.Abstractions.TestService
{
    public interface ITestScoringService
    {
        TestResultDto CalculateScore(
       Dictionary<int, HashSet<int>> correctAnswers,
       List<SubmittedAnswerDto> submittedAnswers);
    }
}
