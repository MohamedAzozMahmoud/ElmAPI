using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class GetQuestionsByIdHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly GetQuestionsByIdHandler _handler;

        public GetQuestionsByIdHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _handler = new GetQuestionsByIdHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetQuestionByIdQuery(99);
            var failureResult = Result<QuestionsDto>.Failure("Question not found", 404);

            _mockRepository
                .Setup(r => r.GetQuestionById(query.id))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Question not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockRepository.Verify(r => r.GetQuestionById(query.id), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenQuestionFound_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetQuestionByIdQuery(1);
            var questionDto = new QuestionsDto
            {
                Id = 1,
                Content = "What is 2 + 2?",
                QuestionType = "MCQ",
                Options = new List<OptionsDto>
                {
                    new OptionsDto { Id = 1, Content = "3", IsCorrect = false },
                    new OptionsDto { Id = 2, Content = "4", IsCorrect = true }
                }
            };
            var successResult = Result<QuestionsDto>.Success(questionDto);

            _mockRepository
                .Setup(r => r.GetQuestionById(query.id))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("What is 2 + 2?", result.Data.Content);
            Assert.Equal("MCQ", result.Data.QuestionType);
        }

        [Fact]
        public async Task Handle_WithValidId_CallsGetQuestionById()
        {
            // Arrange
            var questionId = 5;
            var query = new GetQuestionByIdQuery(questionId);
            var successResult = Result<QuestionsDto>.Success(new QuestionsDto { Id = questionId });

            _mockRepository
                .Setup(r => r.GetQuestionById(questionId))
                .ReturnsAsync(successResult);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetQuestionById(questionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsError_PropagatesErrorMessage()
        {
            // Arrange
            var query = new GetQuestionByIdQuery(1);
            var errorMessage = "Database connection failed";
            var failureResult = Result<QuestionsDto>.Failure(errorMessage, 500);

            _mockRepository
                .Setup(r => r.GetQuestionById(query.id))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsQuestionWithOptions()
        {
            // Arrange
            var query = new GetQuestionByIdQuery(1);
            var questionDto = new QuestionsDto
            {
                Id = 1,
                Content = "Is the earth round?",
                QuestionType = "TrueFalse",
                Options = new List<OptionsDto>
                {
                    new OptionsDto { Id = 1, Content = "True", IsCorrect = true },
                    new OptionsDto { Id = 2, Content = "False", IsCorrect = false }
                }
            };
            var successResult = Result<QuestionsDto>.Success(questionDto);

            _mockRepository
                .Setup(r => r.GetQuestionById(query.id))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Options.Count());
            Assert.Contains(result.Data.Options, o => o.Content == "True" && o.IsCorrect);
        }
    }
}
