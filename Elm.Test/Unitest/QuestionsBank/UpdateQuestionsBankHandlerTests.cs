using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Features.QuestionsBank.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.QuestionsBank
{
    public class UpdateQuestionsBankHandlerTests
    {
        private readonly Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateQuestionsBankHandler _handler;

        public UpdateQuestionsBankHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateQuestionsBankHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBankExists_ReturnsSuccessWithUpdatedDto()
        {
            // Arrange
            var questionsBankId = 1;
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Old Name",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(questionsBankId, "New Name", 2);

            var updatedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "New Name",
                CurriculumId = 2
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = questionsBankId,
                name = "New Name"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(updatedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(updatedQuestionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.name, result.Data.name);
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBankNotFound_ReturnsFailureWith404()
        {
            // Arrange
            var questionsBankId = 999;
            var command = new UpdateQuestionsBankCommand(questionsBankId, "New Name", 1);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync((Elm.Domain.Entities.QuestionsBank?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Questions Bank not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()), Times.Never);
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdatesNameCorrectly()
        {
            // Arrange
            var questionsBankId = 1;
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Original Name",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(questionsBankId, "Updated Name", 1);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync((Elm.Domain.Entities.QuestionsBank qb) => qb);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .Returns(new QuestionsBankDto { Id = questionsBankId, name = "Updated Name" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.QuestionsBank>(qb =>
                qb.Name == "Updated Name")), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdatesCurriculumIdCorrectly()
        {
            // Arrange
            var questionsBankId = 1;
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Test Bank",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(questionsBankId, "Test Bank", 5);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync((Elm.Domain.Entities.QuestionsBank qb) => qb);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .Returns(new QuestionsBankDto { Id = questionsBankId, name = "Test Bank" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.QuestionsBank>(qb =>
                qb.CurriculumId == 5)), Times.Once);
        }

        [Theory]
        [InlineData(1, "Bank A", 1)]
        [InlineData(2, "Bank B", 2)]
        [InlineData(3, "Bank C", 3)]
        public async Task Handle_WithVariousInputs_UpdatesCorrectly(int id, string name, int curriculumId)
        {
            // Arrange
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = id,
                Name = "Old Name",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(id, name, curriculumId);

            var updatedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = id,
                Name = name,
                CurriculumId = curriculumId
            };

            var expectedDto = new QuestionsBankDto
            {
                Id = id,
                name = name
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(updatedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(updatedQuestionsBank))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.name);
        }

        [Fact]
        public async Task Handle_CallsGetByIdFirst()
        {
            // Arrange
            var questionsBankId = 1;
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Test",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(questionsBankId, "New Name", 2);

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(existingQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .Returns(new QuestionsBankDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(questionsBankId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithUpdatedEntity()
        {
            // Arrange
            var questionsBankId = 1;
            var existingQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "Old Name",
                CurriculumId = 1
            };

            var command = new UpdateQuestionsBankCommand(questionsBankId, "New Name", 2);

            var updatedQuestionsBank = new Elm.Domain.Entities.QuestionsBank
            {
                Id = questionsBankId,
                Name = "New Name",
                CurriculumId = 2
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(questionsBankId))
                .ReturnsAsync(existingQuestionsBank);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.QuestionsBank>()))
                .ReturnsAsync(updatedQuestionsBank);

            _mapperMock.Setup(m => m.Map<QuestionsBankDto>(updatedQuestionsBank))
                .Returns(new QuestionsBankDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<QuestionsBankDto>(updatedQuestionsBank), Times.Once);
        }
    }
}
