using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Test.Commands;
using Elm.Application.Contracts.Features.Test.DTOs;
using Elm.Application.Features.Test.Handlers;
using Moq;

namespace Elm.Test.Unitest.Test
{
    public class SubmitTestHandlerTests
    {
        private readonly Mock<ITestSessionService> _mockSessionService;
        private readonly Mock<ITestScoringService> _mockScoringService;
        private readonly SubmitTestHandler _handler;

        public SubmitTestHandlerTests()
        {
            _mockSessionService = new Mock<ITestSessionService>();
            _mockScoringService = new Mock<ITestScoringService>();
            _handler = new SubmitTestHandler(_mockSessionService.Object, _mockScoringService.Object);
        }

        [Fact]
        public async Task Handle_WhenSessionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns((TestSession)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("الجلسة غير موجودة.", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSessionExpired_ReturnsFailureResult()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());
            var expiredSession = new TestSession(
                sessionId,
                1,
                DateTime.UtcNow.AddMinutes(-10), // Expired 10 minutes ago
                new Dictionary<int, HashSet<int>>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(expiredSession);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("انتهى وقت الاختبار.", result.Message);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSessionExpired_RemovesSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());
            var expiredSession = new TestSession(
                sessionId,
                1,
                DateTime.UtcNow.AddMinutes(-10),
                new Dictionary<int, HashSet<int>>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(expiredSession);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSessionService.Verify(s => s.RemoveSession(sessionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsTestResultDto()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var answers = new List<SubmittedAnswerDto>
            {
                new SubmittedAnswerDto { QuestionId = 1, SelectedOptionIds = new List<int> { 1 } },
                new SubmittedAnswerDto { QuestionId = 2, SelectedOptionIds = new List<int> { 4 } }
            };
            var command = new SubmitTestCommand(sessionId, answers);
            var correctAnswers = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 1 } },
                { 2, new HashSet<int> { 4 } }
            };
            var validSession = new TestSession(
                sessionId,
                1,
                DateTime.UtcNow.AddMinutes(10), // Still valid
                correctAnswers);
            var expectedResult = new TestResultDto
            {
                TotalQuestions = 2,
                CorrectAnswers = 2,
                ScorePercentage = 100
            };

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(validSession);

            _mockScoringService
                .Setup(s => s.CalculateScore(correctAnswers, answers))
                .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.TotalQuestions);
            Assert.Equal(2, result.Data.CorrectAnswers);
            Assert.Equal(100, result.Data.ScorePercentage);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsCalculateScore()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var answers = new List<SubmittedAnswerDto>
            {
                new SubmittedAnswerDto { QuestionId = 1, SelectedOptionIds = new List<int> { 1 } }
            };
            var command = new SubmitTestCommand(sessionId, answers);
            var correctAnswers = new Dictionary<int, HashSet<int>> { { 1, new HashSet<int> { 1 } } };
            var validSession = new TestSession(sessionId, 1, DateTime.UtcNow.AddMinutes(10), correctAnswers);

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(validSession);

            _mockScoringService
                .Setup(s => s.CalculateScore(correctAnswers, answers))
                .Returns(new TestResultDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockScoringService.Verify(s => s.CalculateScore(correctAnswers, answers), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_RemovesSessionAfterScoring()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());
            var validSession = new TestSession(
                sessionId,
                1,
                DateTime.UtcNow.AddMinutes(10),
                new Dictionary<int, HashSet<int>>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(validSession);

            _mockScoringService
                .Setup(s => s.CalculateScore(
                    It.IsAny<Dictionary<int, HashSet<int>>>(),
                    It.IsAny<List<SubmittedAnswerDto>>()))
                .Returns(new TestResultDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSessionService.Verify(s => s.RemoveSession(sessionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSessionNotFound_DoesNotCallCalculateScore()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns((TestSession)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockScoringService.Verify(
                s => s.CalculateScore(
                    It.IsAny<Dictionary<int, HashSet<int>>>(),
                    It.IsAny<List<SubmittedAnswerDto>>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenSessionExpired_DoesNotCallCalculateScore()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());
            var expiredSession = new TestSession(
                sessionId,
                1,
                DateTime.UtcNow.AddMinutes(-10),
                new Dictionary<int, HashSet<int>>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(expiredSession);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockScoringService.Verify(
                s => s.CalculateScore(
                    It.IsAny<Dictionary<int, HashSet<int>>>(),
                    It.IsAny<List<SubmittedAnswerDto>>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WithValidSessionId_CallsGetSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new SubmitTestCommand(sessionId, new List<SubmittedAnswerDto>());

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns((TestSession)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSessionService.Verify(s => s.GetSession(sessionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithPartialCorrectAnswers_ReturnsCorrectScore()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var answers = new List<SubmittedAnswerDto>
            {
                new SubmittedAnswerDto { QuestionId = 1, SelectedOptionIds = new List<int> { 1 } },
                new SubmittedAnswerDto { QuestionId = 2, SelectedOptionIds = new List<int> { 3 } } // Wrong answer
            };
            var command = new SubmitTestCommand(sessionId, answers);
            var correctAnswers = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 1 } },
                { 2, new HashSet<int> { 4 } }
            };
            var validSession = new TestSession(sessionId, 1, DateTime.UtcNow.AddMinutes(10), correctAnswers);
            var expectedResult = new TestResultDto
            {
                TotalQuestions = 2,
                CorrectAnswers = 1,
                ScorePercentage = 50
            };

            _mockSessionService
                .Setup(s => s.GetSession(sessionId))
                .Returns(validSession);

            _mockScoringService
                .Setup(s => s.CalculateScore(correctAnswers, answers))
                .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(50, result.Data!.ScorePercentage);
        }
    }
}
