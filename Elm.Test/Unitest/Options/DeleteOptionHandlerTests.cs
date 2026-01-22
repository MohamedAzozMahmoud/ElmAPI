using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Options.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest.Options
{
    public class DeleteOptionHandlerTests
    {
        private readonly Mock<IGenericRepository<Option>> _mockRepository;
        private readonly DeleteOptionHandler _handler;

        public DeleteOptionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<Option>>();
            _handler = new DeleteOptionHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteOptionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.optionId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteOptionCommand(99);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.optionId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete option.", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidOptionId_CallsDeleteAsync()
        {
            // Arrange
            var optionId = 5;
            var command = new DeleteOptionCommand(optionId);

            _mockRepository
                .Setup(r => r.DeleteAsync(optionId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(optionId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task Handle_WithDifferentOptionIds_CallsDeleteAsyncWithCorrectId(int optionId)
        {
            // Arrange
            var command = new DeleteOptionCommand(optionId);

            _mockRepository
                .Setup(r => r.DeleteAsync(optionId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(optionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteReturnsTrue_ReturnsSuccessWithTrueData()
        {
            // Arrange
            var command = new DeleteOptionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.optionId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenDeleteReturnsFalse_ReturnsFailure()
        {
            // Arrange
            var command = new DeleteOptionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.optionId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to delete option", result.Message);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task Handle_ReturnsResultBasedOnRepositoryResponse(bool repositoryResult, bool expectedSuccess)
        {
            // Arrange
            var command = new DeleteOptionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.optionId))
                .ReturnsAsync(repositoryResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedSuccess, result.IsSuccess);
        }
    }
}
