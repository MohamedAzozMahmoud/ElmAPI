using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Test.Commands;
using Elm.Application.Contracts.Features.Test.DTOs;
using Elm.Application.Contracts.Repositories;
using MediatR;

namespace Elm.Application.Features.Test.Handlers
{
    public sealed class StartTestHandler : IRequestHandler<StartTestCommand, Result<TestDataDto>>
    {
        private readonly IQuestionRepository _repository;
        private readonly ITestSessionService _sessionService;

        public StartTestHandler(
            IQuestionRepository repository,
            ITestSessionService sessionService)
        {
            _repository = repository;
            _sessionService = sessionService;
        }

        public async Task<Result<TestDataDto>> Handle(
            StartTestCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Validation
            var bankInfo = await _repository.GetBankInfoAsync(request.QuestionsBankId);

            if (bankInfo is null)
                return Result<TestDataDto>.Failure("بنك الأسئلة غير موجود.", 404);

            if (bankInfo.TotalQuestions < request.NumberOfQuestions)
                return Result<TestDataDto>.Failure(
                    $"عدد الأسئلة غير كافٍ. المتاح: {bankInfo.TotalQuestions}", 400);

            // 2️⃣ Get Random Questions
            var randomIds = await _repository.GetRandomQuestionIdsAsync(
                request.QuestionsBankId,
                request.NumberOfQuestions);

            var questionsData = await _repository.GetQuestionsWithOptionsAsync(randomIds);

            // 3️⃣ Prepare Correct Answers Map
            var correctAnswersMap = questionsData.ToDictionary(
                q => q.Id,
                q => q.Options.Where(o => o.IsCorrect).Select(o => o.Id).ToHashSet()
            );

            // 4️⃣ Create Session
            var durationMinutes = request.NumberOfQuestions * 2;
            var session = _sessionService.CreateSession(
                request.QuestionsBankId,
                durationMinutes,
                correctAnswersMap);

            // 5️⃣ Map to Response DTO (without correct answers!)
            var questions = questionsData.Select(q => new QuestionTestDto
            {
                Id = q.Id,
                Content = q.Content,
                QuestionType = q.QuestionType,
                Options = q.Options
                    .OrderBy(_ => Guid.NewGuid())
                    .Select(o => new OptionTestDto { Id = o.Id, Content = o.Content })
                    .ToList()
            }).ToList();

            return Result<TestDataDto>.Success(new TestDataDto
            {
                TestSessionId = session.Id,
                QuestionBankId = request.QuestionsBankId,
                Duration = TimeSpan.FromMinutes(durationMinutes),
                ExpiresAt = session.ExpiresAt,
                Questions = questions
            });
        }
    }
}
