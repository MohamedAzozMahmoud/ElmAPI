using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Application.Features.Roles.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Roles
{
    public class DeleteRoleHandlerTests
    {
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly DeleteRoleHandler _handler;

        public DeleteRoleHandlerTests()
        {
            var store = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(
                store.Object, null!, null!, null!, null!);
            _handler = new DeleteRoleHandler(_mockRoleManager.Object);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteRoleCommand("NonExistent");

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Role not found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenRoleDeletedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteRoleCommand("Admin");
            var role = new Role { Name = "Admin" };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.DeleteAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteRoleCommand("Admin");
            var role = new Role { Name = "Admin" };
            var errors = new[] { new IdentityError { Description = "Cannot delete role" } };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.DeleteAsync(role))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot delete role", result.Message);
        }

        [Fact]
        public async Task Handle_WhenRoleFound_CallsDeleteAsync()
        {
            // Arrange
            var command = new DeleteRoleCommand("Manager");
            var role = new Role { Name = "Manager" };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.DeleteAsync(role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.DeleteAsync(role), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_DoesNotCallDeleteAsync()
        {
            // Arrange
            var command = new DeleteRoleCommand("NonExistent");

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync((Role)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.DeleteAsync(It.IsAny<Role>()), Times.Never);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("User")]
        [InlineData("Manager")]
        public async Task Handle_WithDifferentRoleNames_CallsFindByNameAsyncWithCorrectName(string roleName)
        {
            // Arrange
            var command = new DeleteRoleCommand(roleName);

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(roleName))
                .ReturnsAsync((Role)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.FindByNameAsync(roleName), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenMultipleErrors_ReturnsAllErrorsInMessage()
        {
            // Arrange
            var command = new DeleteRoleCommand("Admin");
            var role = new Role { Name = "Admin" };
            var errors = new[]
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" }
            };

            _mockRoleManager
                .Setup(r => r.FindByNameAsync(command.Name))
                .ReturnsAsync(role);

            _mockRoleManager
                .Setup(r => r.DeleteAsync(role))
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
