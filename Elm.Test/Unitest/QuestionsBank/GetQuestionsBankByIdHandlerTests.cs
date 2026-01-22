using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.QuestionsBank
{
    public class GetQuestionsBankByIdHandlerTests
    {
        private readonly Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetQuestionsBankByIdHandler _handler;

        public GetQuestionsBankByIdHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetQuestionsBankByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBankExists_ReturnsSuccessWithDto()
        {
            // Arrange
            var questionsBankId = 1;
            var questionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Test Questions Bank",
                CurriculumId = 1
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = questionsBankId,
                name = "Test Questions Bank"
            };

            var query = new GetQuestionsBankByIdQuery(questionsBankId);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(questionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(questionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.name, result.Data.name);
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(questionsBank), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBankNotFound_ReturnsFailureWith404()
        {
            // Arrange
            var questionsBankId = 999;
            var query = new GetQuestionsBankByIdQuery(questionsBankId);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync((Elm.Domain.Entities.QuestionsBank?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Questions Bank not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int questionsBankId)
        {
            // Arrange
            var questionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Test Questions Bank",
                CurriculumId = 1
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = questionsBankId,
                name = "Test Questions Bank"
            };

            var query = new GetQuestionsBankByIdQuery(questionsBankId);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(questionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(questionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(questionsBankId, result.Data?.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var questionsBankId = 1;
            var questionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Mathematics Bank",
                CurriculumId = 2
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = questionsBankId,
                name = "Mathematics Bank"
            };

            var query = new GetQuestionsBankByIdQuery(questionsBankId);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(questionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(questionsBank))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(It.Is<Elm.Domain.Entities.QuestionsBank>(qb =>
                qb.Id == questionsBankId &&
                qb.Name == "Mathematics Bank" &&
                qb.CurriculumId == 2)), Times.Once);
        }
    }
}
