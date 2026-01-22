using Elm.Application.Contracts.Const;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Features.Authentication.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Authentication
{
    public class RegisterStudentHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly RegisterStudentHandler _handler;

        public RegisterStudentHandlerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            _handler = new RegisterStudentHandler(_mockUserManager.Object);
        }



        [Fact]
        public async Task Handle_WhenStudentCreatedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new RegisterStudentCommand("newstudent", "Password123!", "Password123!", "New Student", 1, 2);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Student))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password), Times.Once);
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Student), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenStudentCreated_CreatesUserWithCorrectProperties()
        {
            // Arrange
            var command = new RegisterStudentCommand("teststudent", "Password123!", "Password123!", "Test Student Name", 5, 3);
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
            Assert.Equal("teststudent", capturedUser.UserName);
            Assert.Equal("Test Student Name", capturedUser.FullName);
            Assert.True(capturedUser.IsActived);
            Assert.NotNull(capturedUser.Student);
            Assert.Equal(5, capturedUser.Student.DepartmentId);
            Assert.Equal(3, capturedUser.Student.YearId);
        }

        [Fact]
        public async Task Handle_WhenStudentCreated_AddsStudentRole()
        {
            // Arrange
            var command = new RegisterStudentCommand("newstud", "Password123!", "Password123!", "New Stud", 1, 1);

            _mockUserManager
                .Setup(x => x.FindByNameAsync(command.UserName))
                .ReturnsAsync((AppUser)null!);

            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Student))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Student), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDifferentDepartmentAndYear_CreatesStudentWithCorrectValues()
        {
            // Arrange
            var departmentId = 10;
            var yearId = 4;
            var command = new RegisterStudentCommand("student1", "Password123!", "Password123!", "Student One", departmentId, yearId);
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
            Assert.NotNull(capturedUser.Student);
            Assert.Equal(departmentId, capturedUser.Student.DepartmentId);
            Assert.Equal(yearId, capturedUser.Student.YearId);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ChecksIfUserExistsFirst()
        {
            // Arrange
            var command = new RegisterStudentCommand("checkstudent", "Password123!", "Password123!", "Check Student", 1, 1);

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
