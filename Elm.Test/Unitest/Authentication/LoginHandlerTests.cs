using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Application.Helper;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Elm.Test.Unitest.Authentication
{
    public class LoginHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<IOptions<JWT>> _mockJwtOptions;
        private readonly LoginHandler _handler;
        private readonly JWT _jwtSettings;

        public LoginHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _jwtSettings = new JWT
            {
                Key = "ThisIsASecretKeyForTestingPurposesOnly1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                Duration = 60
            };

            _mockJwtOptions = new Mock<IOptions<JWT>>();
            _mockJwtOptions.Setup(x => x.Value).Returns(_jwtSettings);

            _handler = new LoginHandler(_mockUserManager.Object, _mockJwtOptions.Object);
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new LoginCommand("nonexistent", "password123");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("كلمة المرور او اسم المستخدم غير صحيح", result.Message);
        }

        [Fact]
        public async Task Handle_WhenPasswordInvalid_ReturnsFailureResult()
        {
            // Arrange
            var command = new LoginCommand("testuser", "wrongpassword");
            var user = new AppUser
            {
                Id = "user-id",
                UserName = "testuser",
                FullName = "Test User"
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("كلمة المرور او اسم المستخدم غير صحيح", result.Message);
        }

        [Fact]
        public async Task Handle_WhenLoginSuccessful_ReturnsAuthModelDto()
        {
            // Arrange
            var command = new LoginCommand("testuser", "correctpassword");  // ✅ تصحيح:  حرف صغير
            var user = new AppUser
            {
                Id = "user-id",
                UserName = "testuser",
                FullName = "Test User",
                RefreshTokens = new List<RefreshToken>()
            };
            var roles = new List<string> { "Admin", "User" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _mockUserManager
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // ✅ إضافة Setup للـ Claims إذا كان AuthHelper يستخدمها
            _mockUserManager
                .Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("testuser", result.Data.UserName);
            Assert.Equal("Test User", result.Data.FullName);
            Assert.Equal("user-id", result.Data.UserId);
            Assert.True(result.Data.IsAuthenticated);
            Assert.NotNull(result.Data.Token);
            Assert.NotNull(result.Data.RefreshToken);
        }

        [Fact]
        public async Task Handle_WhenLoginSuccessful_ReturnsCorrectRoles()
        {
            // Arrange
            var command = new LoginCommand("testuser", "correctpassword");
            var user = new AppUser
            {
                Id = "user-id",
                UserName = "testuser",
                FullName = "Test User",
                RefreshTokens = new List<RefreshToken>()
            };
            var roles = new List<string> { "Admin", "Doctor" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _mockUserManager
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);
            // ✅ إضافة Setup للـ Claims إذا كان AuthHelper يستخدمها
            _mockUserManager
                .Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Roles.Count);
            Assert.Contains("Admin", result.Data.Roles);
            Assert.Contains("Doctor", result.Data.Roles);
        }

        [Fact]
        public async Task Handle_WhenUpdateFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new LoginCommand("testuser", "correctpassword");
            var user = new AppUser
            {
                Id = "user-id",
                UserName = "testuser",
                FullName = "Test User",
                RefreshTokens = new List<RefreshToken>()
            };
            var roles = new List<string> { "User" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _mockUserManager
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));
            // ✅ إضافة Setup للـ Claims إذا كان AuthHelper يستخدمها
            _mockUserManager
                .Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("حدث خطأ أثناء تحديث البيانات", result.Message);
        }

        [Fact]
        public async Task Handle_WhenLoginSuccessful_AddsRefreshTokenToUser()
        {
            // Arrange
            var command = new LoginCommand("testuser", "correctpassword");
            var user = new AppUser
            {
                Id = "user-id",
                UserName = "testuser",
                FullName = "Test User",
                RefreshTokens = new List<RefreshToken>()
            };
            var roles = new List<string> { "User" };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, command.Password))
                .ReturnsAsync(true);

            _mockUserManager
                .Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _mockUserManager
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);
            // ✅ إضافة Setup للـ Claims إذا كان AuthHelper يستخدمها
            _mockUserManager
                .Setup(x => x.GetClaimsAsync(user))
                .ReturnsAsync(new List<Claim>());
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(user.RefreshTokens);
            _mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }
}
