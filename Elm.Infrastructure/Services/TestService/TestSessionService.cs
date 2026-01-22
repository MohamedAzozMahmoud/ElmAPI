using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Test.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace Elm.Infrastructure.Services.TestService
{
    public class TestSessionService : ITestSessionService
    {
        private readonly IMemoryCache _cache;
        private const string CacheKeyPrefix = "TestSession_";

        public TestSessionService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TestSession CreateSession(
            int bankId,
            int durationMinutes,
            Dictionary<int, HashSet<int>> correctAnswers)
        {
            var session = new TestSession(
                Id: Guid.NewGuid(),
                QuestionBankId: bankId,
                ExpiresAt: DateTime.UtcNow.AddMinutes(durationMinutes + 1),
                CorrectOptionsByQuestionId: correctAnswers
            );

            _cache.Set(
                $"{CacheKeyPrefix}{session.Id}",
                session,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = session.ExpiresAt
                });

            return session;
        }

        public TestSession? GetSession(Guid sessionId)
        {
            _cache.TryGetValue($"{CacheKeyPrefix}{sessionId}", out TestSession? session);
            return session;
        }

        public void RemoveSession(Guid sessionId)
        {
            _cache.Remove($"{CacheKeyPrefix}{sessionId}");
        }
    }
}
