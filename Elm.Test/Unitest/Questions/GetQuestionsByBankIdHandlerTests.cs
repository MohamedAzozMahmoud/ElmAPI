using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class GetQuestionsByBankIdHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly GetQuestionsByBankIdHandler _handler;

        public GetQuestionsByBankIdHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _handler = new GetQuestionsByBankIdHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionsFound_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(1);
            var questions = new List<QuestionsDto>
            {
                new QuestionsDto
                {
                    Id = 1,
                    Content = "Question 1",
                    QuestionType = "MCQ",
                    Options = new List<OptionsDto>()
                },
                new QuestionsDto
                {
                    Id = 2,
                    Content = "Question 2",
                    QuestionType = "TrueFalse",
                    Options = new List<OptionsDto>()
                }
            };
            var successResult = Result<List<QuestionsDto>>.Success(questions);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task Handle_WhenNoQuestionsFound_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(99);
            var failureResult = Result<List<QuestionsDto>>.Failure("No questions found", 404);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No questions found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WithValidBankId_CallsGetQuestionsByBankId()
        {
            // Arrange
            var bankId = 5;
            var query = new GetAllQuestionsQuery(bankId);
            var successResult = Result<List<QuestionsDto>>.Success(new List<QuestionsDto>());

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(bankId))
                .ReturnsAsync(successResult);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetQuestionsByBankId(bankId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsError_PropagatesErrorMessage()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(1);
            var errorMessage = "Database error occurred";
            var failureResult = Result<List<QuestionsDto>>.Failure(errorMessage, 500);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenEmptyList_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(1);
            var emptyList = new List<QuestionsDto>();
            var successResult = Result<List<QuestionsDto>>.Success(emptyList);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WhenDataIsNull_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(1);
            var failureResult = Result<List<QuestionsDto>>.Failure("Questions bank not found", 404);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Questions bank not found", result.Message);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsQuestionsWithCorrectData()
        {
            // Arrange
            var query = new GetAllQuestionsQuery(1);
            var questions = new List<QuestionsDto>
            {
                new QuestionsDto
                {
                    Id = 1,
                    Content = "What is 2 + 2?",
                    QuestionType = "MCQ",
                    Options = new List<OptionsDto>
                    {
                        new OptionsDto { Id = 1, Content = "3", IsCorrect = false },
                        new OptionsDto { Id = 2, Content = "4", IsCorrect = true }
                    }
                }
            };
            var successResult = Result<List<QuestionsDto>>.Success(questions);

            _mockRepository
                .Setup(r => r.GetQuestionsByBankId(query.questionBankId))
                .ReturnsAsync(successResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data!);
            var question = result.Data.First();
            Assert.Equal("What is 2 + 2?", question.Content);
            Assert.Equal(2, question.Options.Count());
        }
    }
}
