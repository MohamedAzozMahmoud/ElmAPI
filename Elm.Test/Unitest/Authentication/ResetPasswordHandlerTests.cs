using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Authentication
{
    public class ResetPasswordHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly ResetPasswordHandler _handler;

        public ResetPasswordHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new ResetPasswordHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new ResetPasswordCommand("nonexistent", "reset-token", "NewPassword123!");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found.", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockUserManager.Verify(x => x.FindByNameAsync(command.UserName), Times.Once);
            _mockUserManager.Verify(x => x.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPasswordResetSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "valid-reset-token", "NewPassword123!");
            var user = new AppUser
            {
                UserName = "testuser",
                FullName = "Test User"
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenResetPasswordFails_ReturnsFailureWithErrors()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "invalid-token", "NewPassword123!");
            var user = new AppUser
            {
                UserName = "testuser",
                FullName = "Test User"
            };
            var identityErrors = new[]
            {
                new IdentityError { Code = "InvalidToken", Description = "Invalid token" }
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid token", result.Message);
        }

        [Fact]
        public async Task Handle_WhenResetFailsWithMultipleErrors_ReturnsAllErrorMessages()
        {
            // Arrange
            var command = new ResetPasswordCommand("testuser", "expired-token", "weak");
            var user = new AppUser
            {
                UserName = "testuser",
                FullName = "Test User"
            };
            var identityErrors = new[]
            {
                new IdentityError { Code = "InvalidToken", Description = "Token expired" },
                new IdentityError { Code = "PasswordTooWeak", Description = "Password too weak" }
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(user, command.Token, command.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Token expired", result.Message);
            Assert.Contains("Password too weak", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidCommand_CallsResetPasswordAsync()
        {
            // Arrange
            var userName = "resetuser";
            var token = "valid-token-123";
            var newPassword = "NewSecurePassword123!";
            var command = new ResetPasswordCommand(userName, token, newPassword);
            var user = new AppUser { UserName = userName, FullName = "Reset User" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(userName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(user, token, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.ResetPasswordAsync(user, token, newPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidUserName_FindsUserByName()
        {
            // Arrange
            var command = new ResetPasswordCommand("finduser", "token", "NewPass123!");
            var user = new AppUser { UserName = "finduser", FullName = "Find User" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.FindByNameAsync(command.UserName), Times.Once);
        }
    }
}
