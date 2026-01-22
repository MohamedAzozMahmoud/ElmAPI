using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.QuestionsBank
{
    public class DeleteQuestionsBankHandlerTests
    {
        private readonly Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>> _repositoryMock;
        private readonly DeleteQuestionsBankHandler _handler;

        public DeleteQuestionsBankHandlerTests()
        {
            _repositoryMock = new Mock<IGenericRepository<Elm.Domain.Entities.QuestionsBank>>();
            _handler = new DeleteQuestionsBankHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var questionsBankId = 1;
            var command = new DeleteQuestionsBankCommand(questionsBankId);

            _repositoryMock.Setup(r => r.DeleteAsync(questionsBankId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _repositoryMock.Verify(r => r.DeleteAsync(questionsBankId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailureWith500()
        {
            // Arrange
            var questionsBankId = 999;
            var command = new DeleteQuestionsBankCommand(questionsBankId);

            _repositoryMock.Setup(r => r.DeleteAsync(questionsBankId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete Questions Bank", result.Message);
            Assert.Equal(500, result.StatusCode);
            _repositoryMock.Verify(r => r.DeleteAsync(questionsBankId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int questionsBankId)
        {
            // Arrange
            var command = new DeleteQuestionsBankCommand(questionsBankId);

            _repositoryMock.Setup(r => r.DeleteAsync(questionsBankId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.DeleteAsync(questionsBankId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenQuestionsBankNotFound_ReturnsFailure()
        {
            // Arrange
            var nonExistentId = 99999;
            var command = new DeleteQuestionsBankCommand(nonExistentId);

            _repositoryMock.Setup(r => r.DeleteAsync(nonExistentId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete Questions Bank", result.Message);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var questionsBankId = 1;
            var command = new DeleteQuestionsBankCommand(questionsBankId);

            _repositoryMock.Setup(r => r.DeleteAsync(questionsBankId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(questionsBankId), Times.Once);
        }
    }
}
