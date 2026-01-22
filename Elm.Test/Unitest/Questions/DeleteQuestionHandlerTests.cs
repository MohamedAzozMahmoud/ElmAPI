using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class DeleteQuestionHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly DeleteQuestionHandler _handler;

        public DeleteQuestionHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _handler = new DeleteQuestionHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteQuestionCommand(99);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.questionId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Question not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockRepository.Verify(r => r.DeleteAsync(command.questionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenQuestionDeletedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteQuestionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.questionId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockRepository.Verify(r => r.DeleteAsync(command.questionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidQuestionId_CallsDeleteAsync()
        {
            // Arrange
            var questionId = 5;
            var command = new DeleteQuestionCommand(questionId);

            _mockRepository
                .Setup(r => r.DeleteAsync(questionId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(questionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteReturnsFalse_ReturnsNotFoundStatus()
        {
            // Arrange
            var command = new DeleteQuestionCommand(100);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.questionId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenDeleteReturnsTrue_ReturnsSuccessWithTrueData()
        {
            // Arrange
            var command = new DeleteQuestionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.questionId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
