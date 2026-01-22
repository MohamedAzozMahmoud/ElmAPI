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
    public class AddUserPermissionHandlerTests
    {
        private readonly Mock<IUserPerissionRepsitory> _mockUserPermissionRepository;
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockPermissionRepository;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly AddUserPermissionHandler _handler;

        public AddUserPermissionHandlerTests()
        {
            _mockUserPermissionRepository = new Mock<IUserPerissionRepsitory>();
            _mockPermissionRepository = new Mock<IGenericRepository<PermissionsEntity>>();

            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _handler = new AddUserPermissionHandler(
                _mockUserPermissionRepository.Object,
                _mockPermissionRepository.Object,
                _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddUserPermissionCommand("nonexistent", "ReadAccess");

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
        public async Task Handle_WhenPermissionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddUserPermissionCommand("testuser", "NonExistentPermission");
            var user = new AppUser { Id = "user-id", UserName = "testuser" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

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
        public async Task Handle_WhenUserAndPermissionExist_AddsPermissionSuccessfully()
        {
            // Arrange
            var command = new AddUserPermissionCommand("testuser", "ReadAccess");
            var user = new AppUser { Id = "user-id", UserName = "testuser" };
            var permission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserPermissionRepository
                .Setup(r => r.AddUserPermissionAsync(It.IsAny<UserPermissions>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsAddUserPermissionAsync()
        {
            // Arrange
            var command = new AddUserPermissionCommand("testuser", "WriteAccess");
            var user = new AppUser { Id = "user-123", UserName = "testuser" };
            var permission = new PermissionsEntity { Id = 5, Name = "WriteAccess" };
            UserPermissions capturedUserPermission = null!;

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockUserPermissionRepository
                .Setup(r => r.AddUserPermissionAsync(It.IsAny<UserPermissions>()))
                .Callback<UserPermissions>(up => capturedUserPermission = up)
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedUserPermission);
            Assert.Equal("user-123", capturedUserPermission.AppUserId);
            Assert.Equal(5, capturedUserPermission.PermissionId);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_DoesNotCallAddUserPermissionAsync()
        {
            // Arrange
            var command = new AddUserPermissionCommand("nonexistent", "ReadAccess");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync((AppUser)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserPermissionRepository.Verify(r => r.AddUserPermissionAsync(It.IsAny<UserPermissions>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_DoesNotCallAddUserPermissionAsync()
        {
            // Arrange
            var command = new AddUserPermissionCommand("testuser", "NonExistent");
            var user = new AppUser { Id = "user-id", UserName = "testuser" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.userName))
                .ReturnsAsync(user);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync((PermissionsEntity)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserPermissionRepository.Verify(r => r.AddUserPermissionAsync(It.IsAny<UserPermissions>()), Times.Never);
        }
    }
}
