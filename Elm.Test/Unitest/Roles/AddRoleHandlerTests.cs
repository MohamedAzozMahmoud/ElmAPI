using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Commands;
using Elm.Application.Features.Roles.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Roles
{
    public class AddRoleHandlerTests
    {
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly AddRoleHandler _handler;

        public AddRoleHandlerTests()
        {
            var store = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(
                store.Object, null!, null!, null!, null!);
            _handler = new AddRoleHandler(_mockRoleManager.Object);
        }

        [Fact]
        public async Task Handle_WhenRoleCreatedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddRoleCommand("Admin");

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenRoleCreationFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddRoleCommand("Admin");
            var errors = new[] { new IdentityError { Description = "Role already exists" } };

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Role already exists", result.Message);
        }

        [Fact]
        public async Task Handle_WhenCalled_CreatesRoleWithCorrectName()
        {
            // Arrange
            var roleName = "Manager";
            var command = new AddRoleCommand(roleName);
            Role capturedRole = null!;

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
                .Callback<Role>(r => capturedRole = r)
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedRole);
            Assert.Equal(roleName, capturedRole.Name);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsCreateAsync()
        {
            // Arrange
            var command = new AddRoleCommand("User");

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.CreateAsync(It.IsAny<Role>()), Times.Once);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("User")]
        [InlineData("Manager")]
        [InlineData("SuperAdmin")]
        public async Task Handle_WithDifferentRoleNames_CreatesCorrectRole(string roleName)
        {
            // Arrange
            var command = new AddRoleCommand(roleName);
            Role capturedRole = null!;

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
                .Callback<Role>(r => capturedRole = r)
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(roleName, capturedRole.Name);
        }

        [Fact]
        public async Task Handle_WhenMultipleErrors_ReturnsAllErrorsInMessage()
        {
            // Arrange
            var command = new AddRoleCommand("Admin");
            var errors = new[]
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" }
            };

            _mockRoleManager
                .Setup(r => r.CreateAsync(It.IsAny<Role>()))
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
