using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Notifications.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Notifications.Handlers;
using Moq;

namespace Elm.Test.Unitest.Notifications
{
    public class GetUnReadNotificationsCountHandlerTests
    {
        private readonly Mock<INotificationRepository> _mockRepository;
        private readonly GetUnReadNotificationsCountHandler _handler;

        public GetUnReadNotificationsCountHandlerTests()
        {
            _mockRepository = new Mock<INotificationRepository>();
            _handler = new GetUnReadNotificationsCountHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenUnreadNotificationsExist_ReturnsSuccessWithCount()
        {
            // Arrange
            var userId = "user-123";
            var query = new GetUnReadNotificationsCountQuery(userId);
            var expectedCount = 5;

            _mockRepository
                .Setup(r => r.GetUnreadNotificationsCountAsync(userId))
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedCount, result.Data);
        }

        [Fact]
        public async Task Handle_WhenNoUnreadNotifications_ReturnsZero()
        {
            // Arrange
            var userId = "user-456";
            var query = new GetUnReadNotificationsCountQuery(userId);

            _mockRepository
                .Setup(r => r.GetUnreadNotificationsCountAsync(userId))
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task Handle_WithValidUserId_CallsGetUnreadNotificationsCountAsync()
        {
            // Arrange
            var userId = "specific-user-id";
            var query = new GetUnReadNotificationsCountQuery(userId);

            _mockRepository
                .Setup(r => r.GetUnreadNotificationsCountAsync(userId))
                .ReturnsAsync(0);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUnreadNotificationsCountAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData("user-1", 0)]
        [InlineData("user-2", 5)]
        [InlineData("user-3", 100)]
        public async Task Handle_WithDifferentCounts_ReturnsCorrectCount(string userId, int expectedCount)
        {
            // Arrange
            var query = new GetUnReadNotificationsCountQuery(userId);

            _mockRepository
                .Setup(r => r.GetUnreadNotificationsCountAsync(userId))
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedCount, result.Data);
        }

        [Theory]
        [InlineData("user-a")]
        [InlineData("user-b")]
        [InlineData("admin")]
        public async Task Handle_WithDifferentUserIds_CallsRepositoryWithCorrectId(string userId)
        {
            // Arrange
            var query = new GetUnReadNotificationsCountQuery(userId);

            _mockRepository
                .Setup(r => r.GetUnreadNotificationsCountAsync(userId))
                .ReturnsAsync(0);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUnreadNotificationsCountAsync(userId), Times.Once);
        }
    }
}
