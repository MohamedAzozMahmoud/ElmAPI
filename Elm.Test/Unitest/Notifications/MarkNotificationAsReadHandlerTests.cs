using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Notifications.Handlers;
using Moq;

namespace Elm.Test.Unitest.Notifications
{
    public class MarkNotificationAsReadHandlerTests
    {
        private readonly Mock<INotificationRepository> _mockRepository;
        private readonly MarkNotificationAsReadHandler _handler;

        public MarkNotificationAsReadHandlerTests()
        {
            _mockRepository = new Mock<INotificationRepository>();
            _handler = new MarkNotificationAsReadHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenMarkSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand(1);

            _mockRepository
                .Setup(r => r.MarkNotificationAsReadAsync(command.NotificationId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenMarkFails_ReturnsSuccessWithFalse()
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand(99);

            _mockRepository
                .Setup(r => r.MarkNotificationAsReadAsync(command.NotificationId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidNotificationId_CallsMarkNotificationAsReadAsync()
        {
            // Arrange
            var notificationId = 5;
            var command = new MarkNotificationAsReadCommand(notificationId);

            _mockRepository
                .Setup(r => r.MarkNotificationAsReadAsync(notificationId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.MarkNotificationAsReadAsync(notificationId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Handle_WithDifferentNotificationIds_CallsRepositoryWithCorrectId(int notificationId)
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand(notificationId);

            _mockRepository
                .Setup(r => r.MarkNotificationAsReadAsync(notificationId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.MarkNotificationAsReadAsync(notificationId), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ReturnsResultBasedOnRepositoryResponse(bool repositoryResult)
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand(1);

            _mockRepository
                .Setup(r => r.MarkNotificationAsReadAsync(command.NotificationId))
                .ReturnsAsync(repositoryResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(repositoryResult, result.Data);
        }
    }
}
