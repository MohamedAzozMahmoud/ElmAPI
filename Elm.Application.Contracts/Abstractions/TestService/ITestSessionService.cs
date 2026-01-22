using Elm.Application.Contracts.Features.Test.DTOs;

namespace Elm.Application.Contracts.Abstractions.TestService
{
    public interface ITestSessionService
    {
        TestSession CreateSession(int bankId, int durationMinutes, Dictionary<int, HashSet<int>> correctAnswers);
        TestSession? GetSession(Guid sessionId);
        void RemoveSession(Guid sessionId);
    }
}
