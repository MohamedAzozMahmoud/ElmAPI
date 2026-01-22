using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Authentication
{
    public class RegisterDoctorHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly RegisterDoctorHandler _handler;

        public RegisterDoctorHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new RegisterDoctorHandler(_mockUserManager.Object);
        }


        [Fact]
        public async Task Handle_WhenDoctorCreatedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new RegisterDoctorCommand("newdoctor", "Password123!", "Password123!", "New Doctor", "Professor");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Doctor))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password), Times.Once);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Doctor), Times.Once);
        }


        [Fact]
        public async Task Handle_WhenDoctorCreated_CreatesUserWithCorrectProperties()
        {
            // Arrange
            var command = new RegisterDoctorCommand("testdoctor", "Password123!", "Password123!", "Test Doctor Name", "Associate Professor");
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
            Assert.Equal("testdoctor", capturedUser.UserName);
            Assert.Equal("Test Doctor Name", capturedUser.FullName);
            Assert.True(capturedUser.IsActived);
            Assert.NotNull(capturedUser.Doctor);
            Assert.Equal("Associate Professor", capturedUser.Doctor.Title);
        }

        [Fact]
        public async Task Handle_WhenDoctorCreated_AddsDoctorRole()
        {
            // Arrange
            var command = new RegisterDoctorCommand("newdoc", "Password123!", "Password123!", "New Doc", "Dr.");

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Doctor))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Doctor), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDifferentTitles_CreatesUserWithCorrectTitle()
        {
            // Arrange
            var titles = new[] { "Professor", "Dr.", "Associate Professor", "Lecturer" };

            foreach (var title in titles)
            {
                var command = new RegisterDoctorCommand($"doctor_{title}", "Password123!", "Password123!", $"Doctor {title}", title);
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
                Assert.NotNull(capturedUser.Doctor);
                Assert.Equal(title, capturedUser.Doctor.Title);
            }
        }

        [Fact]
        public async Task Handle_WithValidCommand_ChecksIfUserExistsFirst()
        {
            // Arrange
            var command = new RegisterDoctorCommand("checkdoctor", "Password123!", "Password123!", "Check Doctor", "Dr.");

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
