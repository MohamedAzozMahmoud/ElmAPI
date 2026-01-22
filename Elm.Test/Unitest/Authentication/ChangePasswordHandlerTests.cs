using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace Elm.Test.Unitest.Authentication
{
    public class ChangePasswordHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly ChangePasswordHandler _handler;

        public ChangePasswordHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new ChangePasswordHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new ChangePasswordCommand("nonexistent-id", "oldPass", "newPass", "newPass");

            _mockUserManager
                .Setup(x => x.FindByIdAsync(command.UserId))
                .ReturnsAsync((AppUser)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("لا يوجد مستخدم", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockUserManager.Verify(x => x.FindByIdAsync(command.UserId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurrentPasswordInvalid_ReturnsFailureResult()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new ChangePasswordCommand(userId, "wrongPassword", "newPass", "newPass");
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
                .Setup(x => x.CheckPasswordAsync(user, command.currentPassword))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("كلمة المرور الحالية غير صحيحة", result.Message);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(user, command.currentPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenPasswordChangedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new ChangePasswordCommand(userId, "currentPass", "newPass", "newPass");
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
                .Setup(x => x.CheckPasswordAsync(user, command.currentPassword))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.ChangePasswordAsync(user, command.currentPassword, command.newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.ChangePasswordAsync(user, command.currentPassword, command.newPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenChangePasswordFails_ReturnsFailureResult()
        {
            // Arrange
            var userId = "valid-user-id";
            var command = new ChangePasswordCommand(userId, "currentPass", "weakPass", "weakPass");
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
                .Setup(x => x.CheckPasswordAsync(user, command.currentPassword))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.ChangePasswordAsync(user, command.currentPassword, command.newPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));
            // ✅ إضافة Setup للـ Claims إذا كان AuthHelper يستخدمها
            _mockUserManager
                .Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("فشل في تغيير كلمة المرور", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidCredentials_CallsChangePasswordAsync()
        {
            // Arrange
            var userId = "user-123";
            var currentPassword = "OldPassword123!";
            var newPassword = "NewPassword456!";
            var command = new ChangePasswordCommand(userId, currentPassword, newPassword, newPassword);
            var user = new AppUser { Id = userId, UserName = "testuser", FullName = "Test" };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, currentPassword))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserFound_ChecksPasswordBeforeChanging()
        {
            // Arrange
            var userId = "user-123";
            var command = new ChangePasswordCommand(userId, "currentPass", "newPass", "newPass");
            var user = new AppUser { Id = userId, UserName = "testuser", FullName = "Test" };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.currentPassword))
                .ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.CheckPasswordAsync(user, command.currentPassword), Times.Once);
            _mockUserManager.Verify(x => x.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
