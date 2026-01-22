using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;
using PermissionsEntity = Elm.Domain.Entities.Permissions;

namespace Elm.Test.Unitest.Permissions
{
    public class DeleteUserPermissionHandlerTests
    {
        private readonly Mock<IUserPerissionRepsitory> _mockUserPermissionRepository;
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockPermissionRepository;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly DeleteUserPermissionHandler _handler;

        public DeleteUserPermissionHandlerTests()
        {
            _mockUserPermissionRepository = new Mock<IUserPerissionRepsitory>();
            _mockPermissionRepository = new Mock<IGenericRepository<PermissionsEntity>>();

            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _handler = new DeleteUserPermissionHandler(
                _mockUserPermissionRepository.Object,
                _mockPermissionRepository.Object,
                _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("testuser", "NonExistentPermission");

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync((PermissionsEntity)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Permission not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("nonexistent", "ReadAccess");
            var permission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync((AppUser)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenUserAndPermissionExist_DeletesPermissionSuccessfully()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("testuser", "ReadAccess");
            var user = new AppUser { Id = "user-id", UserName = "testuser" };
            var permission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

            _mockUserPermissionRepository
                .Setup(r => r.DeleteUserPermissionAsync(user.Id, permission.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsDeleteUserPermissionAsync()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("testuser", "WriteAccess");
            var user = new AppUser { Id = "user-123", UserName = "testuser" };
            var permission = new PermissionsEntity { Id = 5, Name = "WriteAccess" };

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

            _mockUserPermissionRepository
                .Setup(r => r.DeleteUserPermissionAsync("user-123", 5))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserPermissionRepository.Verify(r => r.DeleteUserPermissionAsync("user-123", 5), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_DoesNotCallDeleteUserPermissionAsync()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("testuser", "NonExistent");

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync((PermissionsEntity)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserPermissionRepository.Verify(r => r.DeleteUserPermissionAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_DoesNotCallDeleteUserPermissionAsync()
        {
            // Arrange
            var command = new DeleteUserPermissionCommand("nonexistent", "ReadAccess");
            var permission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync((AppUser)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserPermissionRepository.Verify(r => r.DeleteUserPermissionAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
