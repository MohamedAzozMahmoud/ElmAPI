using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Notifications.Handlers;
using Moq;

namespace Elm.Test.Unitest.Notifications
{
    public class MarkAllNotificationsAsReadHandlerTests
    {
        private readonly Mock<INotificationRepository> _mockRepository;
        private readonly MarkAllNotificationsAsReadHandler _handler;

        public MarkAllNotificationsAsReadHandlerTests()
        {
            _mockRepository = new Mock<INotificationRepository>();
            _handler = new MarkAllNotificationsAsReadHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenMarkAllSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var userId = "user-123";
            var command = new MarkAllNotificationsAsReadCommand(userId);

            _mockRepository
                .Setup(r => r.MarkAllNotificationsAsReadAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenMarkAllFails_ReturnsSuccessWithFalse()
        {
            // Arrange
            var userId = "user-456";
            var command = new MarkAllNotificationsAsReadCommand(userId);

            _mockRepository
                .Setup(r => r.MarkAllNotificationsAsReadAsync(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidUserId_CallsMarkAllNotificationsAsReadAsync()
        {
            // Arrange
            var userId = "specific-user-id";
            var command = new MarkAllNotificationsAsReadCommand(userId);

            _mockRepository
                .Setup(r => r.MarkAllNotificationsAsReadAsync(userId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.MarkAllNotificationsAsReadAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData("user-1")]
        [InlineData("user-2")]
        [InlineData("admin-user")]
        public async Task Handle_WithDifferentUserIds_CallsRepositoryWithCorrectId(string userId)
        {
            // Arrange
            var command = new MarkAllNotificationsAsReadCommand(userId);

            _mockRepository
                .Setup(r => r.MarkAllNotificationsAsReadAsync(userId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.MarkAllNotificationsAsReadAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ReturnsResultBasedOnRepositoryResponse(bool repositoryResult)
        {
            // Arrange
            var userId = "user-test";
            var command = new MarkAllNotificationsAsReadCommand(userId);

            _mockRepository
                .Setup(r => r.MarkAllNotificationsAsReadAsync(userId))
                .ReturnsAsync(repositoryResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryResult, result.Data);
        }
    }
}
