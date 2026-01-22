using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Permissions
{
    public class GetAllUserPermissionsHandlerTests
    {
        private readonly Mock<IUserPerissionRepsitory> _mockRepository;
        private readonly GetAllUserPermissionsHandler _handler;

        public GetAllUserPermissionsHandlerTests()
        {
            _mockRepository = new Mock<IUserPerissionRepsitory>();
            _handler = new GetAllUserPermissionsHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionsExist_ReturnsSuccessWithPermissions()
        {
            // Arrange
            var userId = "user-123";
            var query = new GetAllUserPermissionsQuery(userId);
            var permissions = new List<GetPermissionsDto>
            {
                new GetPermissionsDto { PermissionId = 1, PermissionName = "Read" },
                new GetPermissionsDto { PermissionId = 2, PermissionName = "Write" }
            };

            _mockRepository
                .Setup(r => r.GetUserPermissionsByUserIdAsync(userId))
                .ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task Handle_WhenNoPermissionsExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = "user-456";
            var query = new GetAllUserPermissionsQuery(userId);
            var emptyPermissions = new List<GetPermissionsDto>();

            _mockRepository
                .Setup(r => r.GetUserPermissionsByUserIdAsync(userId))
                .ReturnsAsync(emptyPermissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidUserId_CallsGetUserPermissionsByUserIdAsync()
        {
            // Arrange
            var userId = "specific-user-id";
            var query = new GetAllUserPermissionsQuery(userId);

            _mockRepository
                .Setup(r => r.GetUserPermissionsByUserIdAsync(userId))
                .ReturnsAsync(new List<GetPermissionsDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUserPermissionsByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsIEnumerableOfGetPermissionsDto()
        {
            // Arrange
            var userId = "user-789";
            var query = new GetAllUserPermissionsQuery(userId);
            var permissions = new List<GetPermissionsDto>
            {
                new GetPermissionsDto { PermissionId = 1, PermissionName = "Admin" }
            };

            _mockRepository
                .Setup(r => r.GetUserPermissionsByUserIdAsync(userId))
                .ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<GetPermissionsDto>>(result.Data);
        }

        [Theory]
        [InlineData("user-1")]
        [InlineData("user-2")]
        [InlineData("admin-user")]
        public async Task Handle_WithDifferentUserIds_CallsRepositoryWithCorrectId(string userId)
        {
            // Arrange
            var query = new GetAllUserPermissionsQuery(userId);

            _mockRepository
                .Setup(r => r.GetUserPermissionsByUserIdAsync(userId))
                .ReturnsAsync(new List<GetPermissionsDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetUserPermissionsByUserIdAsync(userId), Times.Once);
        }
    }
}
