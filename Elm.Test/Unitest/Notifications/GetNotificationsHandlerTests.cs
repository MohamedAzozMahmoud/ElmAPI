using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.DTOs;
using Elm.Application.Contracts.Features.Notifications.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Notifications.Handlers;
using Moq;

namespace Elm.Test.Unitest.Notifications
{
    public class GetNotificationsHandlerTests
    {
        private readonly Mock<INotificationRepository> _mockRepository;
        private readonly GetNotificationsHandler _handler;

        public GetNotificationsHandlerTests()
        {
            _mockRepository = new Mock<INotificationRepository>();
            _handler = new GetNotificationsHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenNotificationsExist_ReturnsSuccessWithNotifications()
        {
            // Arrange
            var userId = "user-123";
            var query = new GetNotificationsQuery(userId);
            var notifications = new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = 1,
                    Title = "Test Notification 1",
                    Message = "Message 1",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                },
                new NotificationDto
                {
                    Id = 2,
                    Title = "Test Notification 2",
                    Message = "Message 2",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockRepository
                .Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(notifications);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task Handle_WhenNoNotificationsExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = "user-456";
            var query = new GetNotificationsQuery(userId);
            var emptyNotifications = new List<NotificationDto>();

            _mockRepository
                .Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(emptyNotifications);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidUserId_CallsGetUserNotificationsAsync()
        {
            // Arrange
            var userId = "specific-user-id";
            var query = new GetNotificationsQuery(userId);

            _mockRepository
                .Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(new List<NotificationDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUserNotificationsAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData("user-1")]
        [InlineData("user-2")]
        [InlineData("admin-user")]
        public async Task Handle_WithDifferentUserIds_CallsRepositoryWithCorrectId(string userId)
        {
            // Arrange
            var query = new GetNotificationsQuery(userId);

            _mockRepository
                .Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(new List<NotificationDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUserNotificationsAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNotificationsWithCorrectData()
        {
            // Arrange
            var userId = "user-test";
            var query = new GetNotificationsQuery(userId);
            var createdAt = DateTime.UtcNow;
            var notifications = new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = 10,
                    Title = "Important Update",
                    Message = "Your file has been approved",
                    IsRead = false,
                    CreatedAt = createdAt
                }
            };

            _mockRepository
                .Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(notifications);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data!);
            var notification = result.Data.First();
            Assert.Equal(10, notification.Id);
            Assert.Equal("Important Update", notification.Title);
            Assert.Equal("Your file has been approved", notification.Message);
            Assert.False(notification.IsRead);
        }
    }
}
