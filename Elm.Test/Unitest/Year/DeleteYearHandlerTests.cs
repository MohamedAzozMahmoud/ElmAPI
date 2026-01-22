using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Year.Handlers;
using Moq;

namespace Elm.Test.Unitest.Year
{
    public class DeleteYearHandlerTests
    {
        private readonly Mock<IYearRepository> _repositoryMock;
        private readonly DeleteYearHandler _handler;

        public DeleteYearHandlerTests()
        {
            _repositoryMock = new Mock<IYearRepository>();
            _handler = new DeleteYearHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var yearId = 1;
            var command = new DeleteYearCommand(yearId);

            _repositoryMock.Setup(r => r.DeleteAsync(yearId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _repositoryMock.Verify(r => r.DeleteAsync(yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailure()
        {
            // Arrange
            var yearId = 999;
            var command = new DeleteYearCommand(yearId);

            _repositoryMock.Setup(r => r.DeleteAsync(yearId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Not deleted", result.Message);
            _repositoryMock.Verify(r => r.DeleteAsync(yearId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int yearId)
        {
            // Arrange
            var command = new DeleteYearCommand(yearId);

            _repositoryMock.Setup(r => r.DeleteAsync(yearId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.DeleteAsync(yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenYearNotFound_ReturnsFailure()
        {
            // Arrange
            var nonExistentId = 99999;
            var command = new DeleteYearCommand(nonExistentId);

            _repositoryMock.Setup(r => r.DeleteAsync(nonExistentId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Not deleted", result.Message);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var yearId = 1;
            var command = new DeleteYearCommand(yearId);

            _repositoryMock.Setup(r => r.DeleteAsync(yearId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(yearId), Times.Once);
        }
    }
}
