using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Test.Commands;
using Elm.Application.Contracts.Features.Test.DTOs;
using MediatR;

namespace Elm.Application.Features.Test.Handlers
{
    public sealed class SubmitTestHandler : IRequestHandler<SubmitTestCommand, Result<TestResultDto>>
    {
        private readonly ITestSessionService _sessionService;
        private readonly ITestScoringService _scoringService;

        public SubmitTestHandler(
            ITestSessionService sessionService,
            ITestScoringService scoringService)
        {
            _sessionService = sessionService;
            _scoringService = scoringService;
        }

        public Task<Result<TestResultDto>> Handle(
            SubmitTestCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Get Session
            var session = _sessionService.GetSession(request.TestSessionId);

            if (session is null)
                return Task.FromResult(
                    Result<TestResultDto>.Failure("الجلسة غير موجودة.", 404));

            // 2️⃣ Check Expiration
            if (session.IsExpired)
            {
                _sessionService.RemoveSession(request.TestSessionId);
                return Task.FromResult(
                    Result<TestResultDto>.Failure("انتهى وقت الاختبار.", 400));
            }

            // 3️⃣ Calculate Score
            var result = _scoringService.CalculateScore(
                session.CorrectOptionsByQuestionId,
                request.Answers);

            // 4️⃣ Remove Session (prevent resubmission)
            _sessionService.RemoveSession(request.TestSessionId);

            return Task.FromResult(Result<TestResultDto>.Success(result));
        }
    }
}
