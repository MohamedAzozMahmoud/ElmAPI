using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class AddRingQuestionHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly AddRingQuestionHandler _handler;

        public AddRingQuestionHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _handler = new AddRingQuestionHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionsAddedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var questionsDtos = new List<AddQuestionsDto>
            {
                new AddQuestionsDto { Content = "Question 1", QuestionType = "MCQ" },
                new AddQuestionsDto { Content = "Question 2", QuestionType = "TrueFalse" }
            };
            var command = new AddRingQuestionsCommand(1, questionsDtos);
            var successResult = Result<bool>.Success(true);

            _mockRepository
                .Setup(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockRepository.Verify(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_ReturnsFailureResult()
        {
            // Arrange
            var questionsDtos = new List<AddQuestionsDto>
            {
                new AddQuestionsDto { Content = "Question 1", QuestionType = "MCQ" }
            };
            var command = new AddRingQuestionsCommand(1, questionsDtos);
            var failureResult = Result<bool>.Failure("Failed to add questions");

            _mockRepository
                .Setup(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add questions", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidBankId_CallsAddRingQuestions()
        {
            // Arrange
            var bankId = 5;
            var questionsDtos = new List<AddQuestionsDto>
            {
                new AddQuestionsDto { Content = "Test", QuestionType = "MCQ" }
            };
            var command = new AddRingQuestionsCommand(bankId, questionsDtos);

            _mockRepository
                .Setup(r => r.AddRingQuestions(bankId, questionsDtos))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestions(bankId, questionsDtos), Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyQuestionsList_PassesEmptyList()
        {
            // Arrange
            var questionsDtos = new List<AddQuestionsDto>();
            var command = new AddRingQuestionsCommand(1, questionsDtos);

            _mockRepository
                .Setup(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestions(1, It.Is<List<AddQuestionsDto>>(l => l.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task Handle_WithMultipleQuestions_PassesAllQuestions()
        {
            // Arrange
            var questionsDtos = new List<AddQuestionsDto>
            {
                new AddQuestionsDto { Content = "Q1", QuestionType = "MCQ" },
                new AddQuestionsDto { Content = "Q2", QuestionType = "MCQ" },
                new AddQuestionsDto { Content = "Q3", QuestionType = "TrueFalse" },
                new AddQuestionsDto { Content = "Q4", QuestionType = "TrueFalse" },
                new AddQuestionsDto { Content = "Q5", QuestionType = "MCQ" }
            };
            var command = new AddRingQuestionsCommand(1, questionsDtos);

            _mockRepository
                .Setup(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestions(1, It.Is<List<AddQuestionsDto>>(l => l.Count == 5)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsRepositoryResult()
        {
            // Arrange
            var questionsDtos = new List<AddQuestionsDto>
            {
                new AddQuestionsDto { Content = "Test", QuestionType = "MCQ" }
            };
            var command = new AddRingQuestionsCommand(1, questionsDtos);
            var expectedResult = Result<bool>.Success(true);

            _mockRepository
                .Setup(r => r.AddRingQuestions(command.questionsBankId, command.QuestionsDtos))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.Equal(expectedResult.Data, result.Data);
        }
    }
}
