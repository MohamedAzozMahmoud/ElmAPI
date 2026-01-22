using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Subject.Handlers;
using Moq;

namespace Elm.Test.Unitest.Subject
{
    public class DeleteSubjectHandlerTests
    {
        private readonly Mock<ISubjectRepository> _repositoryMock;
        private readonly DeleteSubjectHandler _handler;

        public DeleteSubjectHandlerTests()
        {
            _repositoryMock = new Mock<ISubjectRepository>();
            _handler = new DeleteSubjectHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var subjectId = 1;
            var command = new DeleteSubjectCommand(subjectId);

            _repositoryMock.Setup(r => r.DeleteAsync(subjectId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _repositoryMock.Verify(r => r.DeleteAsync(subjectId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailure()
        {
            // Arrange
            var subjectId = 999;
            var command = new DeleteSubjectCommand(subjectId);

            _repositoryMock.Setup(r => r.DeleteAsync(subjectId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete subject.", result.Message);
            _repositoryMock.Verify(r => r.DeleteAsync(subjectId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int subjectId)
        {
            // Arrange
            var command = new DeleteSubjectCommand(subjectId);

            _repositoryMock.Setup(r => r.DeleteAsync(subjectId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.DeleteAsync(subjectId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSubjectNotFound_ReturnsFailure()
        {
            // Arrange
            var nonExistentId = 99999;
            var command = new DeleteSubjectCommand(nonExistentId);

            _repositoryMock.Setup(r => r.DeleteAsync(nonExistentId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete subject.", result.Message);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var subjectId = 1;
            var command = new DeleteSubjectCommand(subjectId);

            _repositoryMock.Setup(r => r.DeleteAsync(subjectId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(subjectId), Times.Once);
        }
    }
}
