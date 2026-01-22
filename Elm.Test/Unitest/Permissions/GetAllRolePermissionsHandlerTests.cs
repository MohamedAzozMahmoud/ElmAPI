using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Permissions
{
    public class GetAllRolePermissionsHandlerTests
    {
        private readonly Mock<IRolePermissionRepository> _mockRepository;
        private readonly GetAllRolePermissionsHandler _handler;

        public GetAllRolePermissionsHandlerTests()
        {
            _mockRepository = new Mock<IRolePermissionRepository>();
            _handler = new GetAllRolePermissionsHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionsExist_ReturnsSuccessWithPermissions()
        {
            // Arrange
            var roleId = "role-123";
            var query = new GetAllRolePermissionsQuery(roleId);
            var permissions = new List<GetPermissionsDto>
            {
                new GetPermissionsDto { PermissionId = 1, PermissionName = "Read" },
                new GetPermissionsDto { PermissionId = 2, PermissionName = "Write" },
                new GetPermissionsDto { PermissionId = 3, PermissionName = "Delete" }
            };

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());
        }

        [Fact]
        public async Task Handle_WhenNoPermissionsExist_ReturnsEmptyList()
        {
            // Arrange
            var roleId = "role-456";
            var query = new GetAllRolePermissionsQuery(roleId);
            var emptyPermissions = new List<GetPermissionsDto>();

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(emptyPermissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithValidRoleId_CallsGetPermissionsByRoleIdAsync()
        {
            // Arrange
            var roleId = "specific-role-id";
            var query = new GetAllRolePermissionsQuery(roleId);

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(new List<GetPermissionsDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetPermissionsByRoleIdAsync(roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsIEnumerableOfGetPermissionsDto()
        {
            // Arrange
            var roleId = "role-789";
            var query = new GetAllRolePermissionsQuery(roleId);
            var permissions = new List<GetPermissionsDto>
            {
                new GetPermissionsDto { PermissionId = 1, PermissionName = "Admin" }
            };

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<GetPermissionsDto>>(result.Data);
        }

        [Theory]
        [InlineData("role-1")]
        [InlineData("role-2")]
        [InlineData("admin-role")]
        public async Task Handle_WithDifferentRoleIds_CallsRepositoryWithCorrectId(string roleId)
        {
            // Arrange
            var query = new GetAllRolePermissionsQuery(roleId);

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(new List<GetPermissionsDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetPermissionsByRoleIdAsync(roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsCorrectPermissionData()
        {
            // Arrange
            var roleId = "role-test";
            var query = new GetAllRolePermissionsQuery(roleId);
            var permissions = new List<GetPermissionsDto>
            {
                new GetPermissionsDto { PermissionId = 10, PermissionName = "FullAccess" },
                new GetPermissionsDto { PermissionId = 20, PermissionName = "ReadOnly" }
            };

            _mockRepository
                .Setup(r => r.GetPermissionsByRoleIdAsync(roleId))
                .ReturnsAsync(permissions);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var permissionList = result.Data!.ToList();
            Assert.Equal(2, permissionList.Count);
            Assert.Contains(permissionList, p => p.PermissionId == 10 && p.PermissionName == "FullAccess");
            Assert.Contains(permissionList, p => p.PermissionId == 20 && p.PermissionName == "ReadOnly");
        }
    }
}
