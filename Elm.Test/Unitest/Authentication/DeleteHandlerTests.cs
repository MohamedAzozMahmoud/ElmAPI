using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Authentication
{
    public class DeleteHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly DeleteHandler _handler;

        public DeleteHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new DeleteHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteCommand("nonexistent-user-id");

            _mockUserManager
                .Setup(x => x.FindByIdAsync(command.UserId))
                .ReturnsAsync((AppUser)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found.", result.Message);
            _mockUserManager.Verify(x => x.FindByIdAsync(command.UserId), Times.Once);
            _mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<AppUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserDeletedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new DeleteCommand(userId);
            var user = new AppUser
            {
                Id = userId,
                UserName = "testuser",
                FullName = "Test User"
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            _mockUserManager.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailureWithErrors()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new DeleteCommand(userId);
            var user = new AppUser
            {
                Id = userId,
                UserName = "testuser",
                FullName = "Test User"
            };
            var identityErrors = new[]
            {
                new IdentityError { Code = "DeleteError", Description = "Delete operation failed" }
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Delete operation failed", result.Message);
            _mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
            _mockUserManager.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteFailsWithMultipleErrors_ReturnsAllErrorMessages()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new DeleteCommand(userId);
            var user = new AppUser
            {
                Id = userId,
                UserName = "testuser",
                FullName = "Test User"
            };
            var identityErrors = new[]
            {
                new IdentityError { Code = "Error1", Description = "First error" },
                new IdentityError { Code = "Error2", Description = "Second error" }
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("First error", result.Message);
            Assert.Contains("Second error", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidUserId_CallsFindByIdAsync()
        {
            // Arrange
            var userId = "test-user-123";
            var command = new DeleteCommand(userId);
            var user = new AppUser { Id = userId, UserName = "testuser", FullName = "Test" };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.FindByIdAsync(userId), Times.Once);
        }
    }
}
