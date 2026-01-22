using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Authentication
{
    public class RegisterHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new RegisterHandler(_mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_WhenUserCreatedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new RegisterCommand("newuser", "Password123!", "Password123!", "New User");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password), Times.Once);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUserCreated_CreatesUserWithCorrectProperties()
        {
            // Arrange
            var command = new RegisterCommand("testuser", "Password123!", "Password123!", "Test Full Name");
            AppUser capturedUser = null!;

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .Callback<AppUser, string>((user, _) => capturedUser = user)
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedUser);
            Assert.Equal("testuser", capturedUser.UserName);
            Assert.Equal("Test Full Name", capturedUser.FullName);
            Assert.True(capturedUser.IsActived);
        }

        [Fact]
        public async Task Handle_WhenUserCreated_AddsAdminRole()
        {
            // Arrange
            var command = new RegisterCommand("newadmin", "Password123!", "Password123!", "New Admin");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ChecksIfUserExistsFirst()
        {
            // Arrange
            var command = new RegisterCommand("checkuser", "Password123!", "Password123!", "Check User");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.FindByNameAsync(command.UserName), Times.Once);
        }
    }
}
