using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.QuestionsBank
{
    public class GetAllQuestionsBanksHandlerTests
    {
        private readonly Mock<IQuestionBankRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllQuestionsBanksHandler _handler;

        public GetAllQuestionsBanksHandlerTests()
        {
            _repositoryMock = new Mock<IQuestionBankRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllQuestionsBanksHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBanksExist_ReturnsSuccessWithList()
        {
            // Arrange
            var curriculumId = 1;
            var questionsBanks = new List<QuestionsBankDto>
            {
                new QuestionsBankDto { Id = 1, name = "Math Bank" },
                new QuestionsBankDto { Id = 2, name = "Physics Bank" },
                new QuestionsBankDto { Id = 3, name = "Chemistry Bank" }
            };

            var query = new GetAllQuestionsBankQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(Result<List<QuestionsBankDto>>.Success(questionsBanks));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Math Bank", result.Data[0].name);
            Assert.Equal("Physics Bank", result.Data[1].name);
            Assert.Equal("Chemistry Bank", result.Data[2].name);
            _repositoryMock.Verify(r => r.GetQuestionsBank(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoQuestionsBanksExist_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var curriculumId = 999;
            var emptyQuestionsBanks = new List<QuestionsBankDto>();

            var query = new GetAllQuestionsBankQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(Result<List<QuestionsBankDto>>.Success(emptyQuestionsBanks));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            _repositoryMock.Verify(r => r.GetQuestionsBank(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryFails_ReturnsFailure()
        {
            // Arrange
            var curriculumId = 1;
            var errorMessage = "Database error";

            var query = new GetAllQuestionsBankQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(Result<List<QuestionsBankDto>>.Failure(errorMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(errorMessage, result.Message);
            _repositoryMock.Verify(r => r.GetQuestionsBank(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNullData_ReturnsFailure()
        {
            // Arrange
            var curriculumId = 1;

            var query = new GetAllQuestionsBankQuery(curriculumId);

            var failureResult = Result<List<QuestionsBankDto>>.Failure("No data found");
            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task Handle_WithVariousCurriculumIds_CallsRepositoryWithCorrectId(int curriculumId)
        {
            // Arrange
            var questionsBanks = new List<QuestionsBankDto>
            {
                new QuestionsBankDto { Id = 1, name = "Test Bank" }
            };

            var query = new GetAllQuestionsBankQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(Result<List<QuestionsBankDto>>.Success(questionsBanks));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetQuestionsBank(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var curriculumId = 1;
            var questionsBanks = new List<QuestionsBankDto>();

            var query = new GetAllQuestionsBankQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetQuestionsBank(curriculumId))
                .ReturnsAsync(Result<List<QuestionsBankDto>>.Success(questionsBanks));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetQuestionsBank(It.IsAny<int>()), Times.Once);
        }
    }
}
