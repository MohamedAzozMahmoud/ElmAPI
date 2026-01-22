using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;
using PermissionsEntity = Elm.Domain.Entities.Permissions;

namespace Elm.Test.Unitest.Permissions
{
    public class AddPermissionHandlerTests
    {
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddPermissionHandler _handler;

        public AddPermissionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<PermissionsEntity>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new AddPermissionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionAddedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddPermissionCommand("ReadAccess");
            var addedPermission = new PermissionsEntity { Id = 1, Name = "ReadAccess" };
            var permissionDto = new PermissionDto { Id = 1, Name = "ReadAccess" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync(addedPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(addedPermission))
                .Returns(permissionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("ReadAccess", result.Data.Name);
        }

        [Fact]
        public async Task Handle_WhenCalled_CreatesPermissionWithCorrectName()
        {
            // Arrange
            var permissionName = "WriteAccess";
            var command = new AddPermissionCommand(permissionName);
            PermissionsEntity capturedPermission = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<PermissionsEntity>()))
                .Callback<PermissionsEntity>(p => capturedPermission = p)
                .ReturnsAsync((PermissionsEntity p) => p);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedPermission);
            Assert.Equal(permissionName, capturedPermission.Name);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsAddAsync()
        {
            // Arrange
            var command = new AddPermissionCommand("DeleteAccess");

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync(new PermissionsEntity { Id = 1, Name = "DeleteAccess" });

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<PermissionsEntity>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToPermissionDto()
        {
            // Arrange
            var command = new AddPermissionCommand("AdminAccess");
            var addedPermission = new PermissionsEntity { Id = 5, Name = "AdminAccess" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync(addedPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(addedPermission))
                .Returns(new PermissionDto { Id = 5, Name = "AdminAccess" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<PermissionDto>(addedPermission), Times.Once);
        }

        [Theory]
        [InlineData("Read")]
        [InlineData("Write")]
        [InlineData("Delete")]
        [InlineData("Admin")]
        public async Task Handle_WithDifferentPermissionNames_CreatesCorrectPermission(string permissionName)
        {
            // Arrange
            var command = new AddPermissionCommand(permissionName);
            PermissionsEntity capturedPermission = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<PermissionsEntity>()))
                .Callback<PermissionsEntity>(p => capturedPermission = p)
                .ReturnsAsync((PermissionsEntity p) => p);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto { Name = permissionName });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(permissionName, capturedPermission.Name);
        }
    }
}
