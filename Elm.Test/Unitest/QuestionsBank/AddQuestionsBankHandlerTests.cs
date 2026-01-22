using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.QuestionsBank
{
    public class AddQuestionsBankHandlerTests
    {
        private readonly Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddQuestionsBankHandler _handler;

        public AddQuestionsBankHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddQuestionsBankHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsSuccessWithDto()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Math Questions Bank", 1);

            var addedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = 1,
                Name = "Math Questions Bank",
                CurriculumId = 1
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = 1,
                name = "Math Questions Bank"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(addedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(addedQuestionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.name, result.Data.name);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Elm.Domain.Entities.QuestionsBank>(qb =>
                qb.Name == command.name &&
                qb.CurriculumId == command.curriculumId)), Times.Once);
        }

        [Theory]
        [InlineData("Physics Bank", 1)]
        [InlineData("Chemistry Bank", 2)]
        [InlineData("Biology Bank", 3)]
        public async Task Handle_WithVariousInputs_CreatesCorrectQuestionsBank(string name, int curriculumId)
        {
            // Arrange
            var command = new AddQuestionsBankCommand(name, curriculumId);

            var addedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = 1,
                Name = name,
                CurriculumId = curriculumId
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = 1,
                name = name
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(addedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(addedQuestionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.name);
        }

        [Fact]
        public async Task Handle_CallsRepositoryWithCorrectEntity()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Test Bank", 5);
            Elm.Domain.Entities.QuestionsBank? capturedQuestionsBank = null;

            var addedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = 1,
                Name = "Test Bank",
                CurriculumId = 5
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = 1,
                name = "Test Bank"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .Callback<Elm.Domain.Entities.QuestionsBank>(qb => capturedQuestionsBank = qb)
                .ReturnsAsync(addedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(addedQuestionsBank))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedQuestionsBank);
            Assert.Equal(command.name, capturedQuestionsBank.Name);
            Assert.Equal(command.curriculumId, capturedQuestionsBank.CurriculumId);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("English Bank", 3);

            var addedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = 10,
                Name = "English Bank",
                CurriculumId = 3
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = 10,
                name = "English Bank"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(addedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(addedQuestionsBank))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(addedQuestionsBank), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Test Bank", 1);

            var addedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = 1,
                Name = "Test Bank",
                CurriculumId = 1
            };

            var expectedDto = new QuestionsBankDto();

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(addedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()), Times.Once);
        }
    }
}
