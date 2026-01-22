using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Features.Test.Commands;
using Elm.Application.Contracts.Features.Test.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Test.Handlers;
using Moq;

namespace Elm.Test.Unitest.Test
{
    public class StartTestHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly Mock<ITestSessionService> _mockSessionService;
        private readonly StartTestHandler _handler;

        public StartTestHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _mockSessionService = new Mock<ITestSessionService>();
            _handler = new StartTestHandler(_mockRepository.Object, _mockSessionService.Object);
        }

        [Fact]
        public async Task Handle_WhenBankNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new StartTestCommand(99, 10);

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync((QuestionBankInfo)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("بنك الأسئلة غير موجود.", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenInsufficientQuestions_ReturnsFailureResult()
        {
            // Arrange
            var command = new StartTestCommand(1, 20);
            var bankInfo = new QuestionBankInfo(10); // Only 10 questions available

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync(bankInfo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("عدد الأسئلة غير كافٍ", result.Message);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsTestDataDto()
        {
            // Arrange
            var command = new StartTestCommand(1, 5);
            var bankInfo = new QuestionBankInfo(10);
            var questionIds = new List<int> { 1, 2, 3, 4, 5 };
            var questionsWithOptions = new List<QuestionWithOptions>
            {
                new QuestionWithOptions(1, "Question 1", "MultipleChoice", new List<OptionData>
                {
                    new OptionData(1, "Option A", true),
                    new OptionData(2, "Option B", false)
                }),
                new QuestionWithOptions(2, "Question 2", "MultipleChoice", new List<OptionData>
                {
                    new OptionData(3, "Option C", false),
                    new OptionData(4, "Option D", true)
                })
            };
            var sessionId = Guid.NewGuid();
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            var session = new TestSession(sessionId, 1, expiresAt, new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 1 } },
                { 2, new HashSet<int> { 4 } }
            });

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync(bankInfo);

            _mockRepository
                .Setup(r => r.GetRandomQuestionIdsAsync(command.QuestionsBankId, command.NumberOfQuestions))
                .ReturnsAsync(questionIds);

            _mockRepository
                .Setup(r => r.GetQuestionsWithOptionsAsync(questionIds))
                .ReturnsAsync(questionsWithOptions);

            _mockSessionService
                .Setup(s => s.CreateSession(
                    command.QuestionsBankId,
                    It.IsAny<int>(),
                    It.IsAny<Dictionary<int, HashSet<int>>>()))
                .Returns(session);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(sessionId, result.Data.TestSessionId);
            Assert.Equal(command.QuestionsBankId, result.Data.QuestionBankId);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsGetBankInfoAsync()
        {
            // Arrange
            var command = new StartTestCommand(1, 5);

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync((QuestionBankInfo)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetBankInfoAsync(command.QuestionsBankId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenBankNotFound_DoesNotCallGetRandomQuestionIds()
        {
            // Arrange
            var command = new StartTestCommand(1, 5);

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync((QuestionBankInfo)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetRandomQuestionIdsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenInsufficientQuestions_DoesNotCallGetRandomQuestionIds()
        {
            // Arrange
            var command = new StartTestCommand(1, 20);
            var bankInfo = new QuestionBankInfo(10);

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync(bankInfo);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetRandomQuestionIdsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CreateSessionWithCorrectDuration()
        {
            // Arrange
            var command = new StartTestCommand(1, 5);
            var bankInfo = new QuestionBankInfo(10);
            var questionIds = new List<int> { 1, 2, 3, 4, 5 };
            var questionsWithOptions = new List<QuestionWithOptions>
            {
                new QuestionWithOptions(1, "Q1", "MCQ", new List<OptionData>
                {
                    new OptionData(1, "A", true)
                })
            };
            var expectedDuration = 5 * 2; // NumberOfQuestions * 2
            var session = new TestSession(Guid.NewGuid(), 1, DateTime.UtcNow.AddMinutes(expectedDuration), new Dictionary<int, HashSet<int>>());

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(command.QuestionsBankId))
                .ReturnsAsync(bankInfo);

            _mockRepository
                .Setup(r => r.GetRandomQuestionIdsAsync(command.QuestionsBankId, command.NumberOfQuestions))
                .ReturnsAsync(questionIds);

            _mockRepository
                .Setup(r => r.GetQuestionsWithOptionsAsync(questionIds))
                .ReturnsAsync(questionsWithOptions);

            _mockSessionService
                .Setup(s => s.CreateSession(
                    command.QuestionsBankId,
                    expectedDuration,
                    It.IsAny<Dictionary<int, HashSet<int>>>()))
                .Returns(session);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockSessionService.Verify(s => s.CreateSession(
                command.QuestionsBankId,
                expectedDuration,
                It.IsAny<Dictionary<int, HashSet<int>>>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 10)]
        [InlineData(3, 3)]
        public async Task Handle_WithDifferentParameters_CallsRepositoryWithCorrectValues(int bankId, int numberOfQuestions)
        {
            // Arrange
            var command = new StartTestCommand(bankId, numberOfQuestions);

            _mockRepository
                .Setup(r => r.GetBankInfoAsync(bankId))
                .ReturnsAsync((QuestionBankInfo)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetBankInfoAsync(bankId), Times.Once);
        }
    }
}
