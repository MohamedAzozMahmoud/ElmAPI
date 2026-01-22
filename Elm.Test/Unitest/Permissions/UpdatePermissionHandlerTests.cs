using AutoMapper;
using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;
using PermissionsEntity = Elm.Domain.Entities.Permissions;

namespace Elm.Test.Unitest.Permissions
{
    public class UpdatePermissionHandlerTests
    {
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdatePermissionHandler _handler;

        public UpdatePermissionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<PermissionsEntity>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdatePermissionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdatePermissionCommand(99, "UpdatedPermission");

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((PermissionsEntity)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Permission not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<PermissionsEntity>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenPermissionFound_UpdatesSuccessfully()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "UpdatedPermission");
            var existingPermission = new PermissionsEntity { Id = 1, Name = "OldPermission" };
            var permissionDto = new PermissionDto { Id = 1, Name = "UpdatedPermission" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingPermission);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingPermission))
                .ReturnsAsync(existingPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(existingPermission))
                .Returns(permissionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("UpdatedPermission", result.Data.Name);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesPermissionName()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "NewName");
            var existingPermission = new PermissionsEntity { Id = 1, Name = "OldName" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingPermission);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync((PermissionsEntity p) => p);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("NewName", existingPermission.Name);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsUpdateAsync()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "UpdatedName");
            var existingPermission = new PermissionsEntity { Id = 1, Name = "OldName" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingPermission);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingPermission))
                .ReturnsAsync(existingPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(existingPermission), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToPermissionDto()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "Updated");
            var existingPermission = new PermissionsEntity { Id = 1, Name = "Original" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingPermission);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync(existingPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(existingPermission))
                .Returns(new PermissionDto { Id = 1, Name = "Updated" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<PermissionDto>(existingPermission), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidId_CallsGetByIdAsync()
        {
            // Arrange
            var permissionId = 5;
            var command = new UpdatePermissionCommand(permissionId, "NewName");
            var existingPermission = new PermissionsEntity { Id = permissionId, Name = "OldName" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(permissionId))
                .ReturnsAsync(existingPermission);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<PermissionsEntity>()))
                .ReturnsAsync(existingPermission);

            _mockMapper
                .Setup(m => m.Map<PermissionDto>(It.IsAny<PermissionsEntity>()))
                .Returns(new PermissionDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(permissionId), Times.Once);
        }
    }
}
