using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Notifications.Handlers;
using Moq;

namespace Elm.Test.Unitest.Notifications
{
    public class DeleteNotificationHandlerTests
    {
        private readonly Mock<INotificationRepository> _mockRepository;
        private readonly DeleteNotificationHandler _handler;

        public DeleteNotificationHandlerTests()
        {
            _mockRepository = new Mock<INotificationRepository>();
            _handler = new DeleteNotificationHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var command = new DeleteNotificationCommand(1);

            _mockRepository
                .Setup(r => r.DeleteNotificationAsync(command.NotificationId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsSuccessWithFalse()
        {
            // Arrange
            var command = new DeleteNotificationCommand(99);

            _mockRepository
                .Setup(r => r.DeleteNotificationAsync(command.NotificationId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidNotificationId_CallsDeleteNotificationAsync()
        {
            // Arrange
            var notificationId = 5;
            var command = new DeleteNotificationCommand(notificationId);

            _mockRepository
                .Setup(r => r.DeleteNotificationAsync(notificationId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteNotificationAsync(notificationId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Handle_WithDifferentNotificationIds_CallsRepositoryWithCorrectId(int notificationId)
        {
            // Arrange
            var command = new DeleteNotificationCommand(notificationId);

            _mockRepository
                .Setup(r => r.DeleteNotificationAsync(notificationId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteNotificationAsync(notificationId), Times.Once);
        }
    }
}
