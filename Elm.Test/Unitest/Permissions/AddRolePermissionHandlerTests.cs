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
    public class AddRolePermissionHandlerTests
    {
        private readonly Mock<IRolePermissionRepository> _mockRolePermissionRepository;
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockPermissionRepository;
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly AddRolePermissionHandler _handler;

        public AddRolePermissionHandlerTests()
        {
            _mockRolePermissionRepository = new Mock<IRolePermissionRepository>();
            _mockPermissionRepository = new Mock<IGenericRepository<PermissionsEntity>>();

            var store = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(
                store.Object, null!, null!, null!, null!);

            _handler = new AddRolePermissionHandler(
                _mockRolePermissionRepository.Object,
                _mockRoleManager.Object,
                _mockPermissionRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddRolePermissionCommand("NonExistentRole", "ReadAccess");

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Role not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddRolePermissionCommand("Admin", "NonExistentPermission");
            var role = new Role { Id = "role-id", Name = "Admin" };

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync(role);

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
        public async Task Handle_WhenRoleAndPermissionExist_AddsPermissionSuccessfully()
        {
            // Arrange
            var command = new AddRolePermissionCommand("Admin", "ReadAccess");
            var role = new Role { Id = "role-id", Name = "Admin" };
            var permission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync(role);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockRolePermissionRepository
                .Setup(r => r.AddRolePermissionAsync(It.IsAny<RolePermissions>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsAddRolePermissionAsync()
        {
            // Arrange
            var command = new AddRolePermissionCommand("Manager", "WriteAccess");
            var role = new Role { Id = "role-123", Name = "Manager" };
            var permission = new PermissionsEntity { Id = 5, Name = "WriteAccess" };
            RolePermissions capturedRolePermission = null!;

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync(role);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync(permission);

            _mockRolePermissionRepository
                .Setup(r => r.AddRolePermissionAsync(It.IsAny<RolePermissions>()))
                .Callback<RolePermissions>(rp => capturedRolePermission = rp)
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedRolePermission);
            Assert.Equal("role-123", capturedRolePermission.RoleId);
            Assert.Equal(5, capturedRolePermission.PermissionId);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_DoesNotCallAddRolePermissionAsync()
        {
            // Arrange
            var command = new AddRolePermissionCommand("NonExistent", "ReadAccess");

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync((Role)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRolePermissionRepository.Verify(r => r.AddRolePermissionAsync(It.IsAny<RolePermissions>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_DoesNotCallAddRolePermissionAsync()
        {
            // Arrange
            var command = new AddRolePermissionCommand("Admin", "NonExistent");
            var role = new Role { Id = "role-id", Name = "Admin" };

            _mockRoleManager
                .Setup(x => x.FindByNameAsync(command.roleName))
                .ReturnsAsync(role);

            _mockPermissionRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<PermissionsEntity, bool>>>()))
                .ReturnsAsync((PermissionsEntity)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRolePermissionRepository.Verify(r => r.AddRolePermissionAsync(It.IsAny<RolePermissions>()), Times.Never);
        }
    }
}
