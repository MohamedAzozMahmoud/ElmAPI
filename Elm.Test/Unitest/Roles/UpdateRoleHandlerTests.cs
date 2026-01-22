using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Application.Features.Roles.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Roles
{
    public class UpdateRoleHandlerTests
    {
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly UpdateRoleHandler _handler;

        public UpdateRoleHandlerTests()
        {
            var store = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(
                store.Object, null!, null!, null!, null!);
            _handler = new UpdateRoleHandler(_mockRoleManager.Object);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateRoleCommand("NonExistent", "NewName");

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Role not found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenRoleUpdatedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new UpdateRoleCommand("OldName", "NewName");
            var role = new Role { Name = "OldName" };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateRoleCommand("OldName", "NewName");
            var role = new Role { Name = "OldName" };
            var errors = new[] { new IdentityError { Description = "Update failed" } };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Update failed", result.Message);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesRoleName()
        {
            // Arrange
            var command = new UpdateRoleCommand("OldName", "NewName");
            var role = new Role { Name = "OldName" };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("NewName", role.Name);
        }

        [Fact]
        public async Task Handle_WhenRoleFound_CallsUpdateAsync()
        {
            // Arrange
            var command = new UpdateRoleCommand("OldName", "NewName");
            var role = new Role { Name = "OldName" };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.UpdateAsync(role), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_DoesNotCallUpdateAsync()
        {
            // Arrange
            var command = new UpdateRoleCommand("NonExistent", "NewName");

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync((Role)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.UpdateAsync(It.IsAny<Role>()), Times.Never);
        }

        [Theory]
        [InlineData("Admin", "SuperAdmin")]
        [InlineData("User", "Member")]
        [InlineData("Manager", "TeamLead")]
        public async Task Handle_WithDifferentNames_UpdatesRoleCorrectly(string oldName, string newName)
        {
            // Arrange
            var command = new UpdateRoleCommand(oldName, newName);
            var role = new Role { Name = oldName };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newName, role.Name);
        }

        [Fact]
        public async Task Handle_CallsFindByNameAsyncWithOldName()
        {
            // Arrange
            var oldName = "OriginalRole";
            var command = new UpdateRoleCommand(oldName, "NewRole");

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(oldName))
                .ReturnsAsync((Role)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.FindByNameAsync(oldName), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenMultipleErrors_ReturnsAllErrorsInMessage()
        {
            // Arrange
            var command = new UpdateRoleCommand("OldName", "NewName");
            var role = new Role { Name = "OldName" };
            var errors = new[]
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" }
            };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.oldName))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.UpdateAsync(role))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error 1", result.Message);
            Assert.Contains("Error 2", result.Message);
        }
    }
}
