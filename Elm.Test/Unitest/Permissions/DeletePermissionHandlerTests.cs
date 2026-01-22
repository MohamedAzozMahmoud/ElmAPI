using Elm.Application.Contracts.Features.Permissions.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;
using PermissionsEntity = Elm.Domain.Entities.Permissions;

namespace Elm.Test.Unitest.Permissions
{
    public class DeletePermissionHandlerTests
    {
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockRepository;
        private readonly DeletePermissionHandler _handler;

        public DeletePermissionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<PermissionsEntity>>();
            _handler = new DeletePermissionHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenCalled_DeletesPermissionSuccessfully()
        {
            // Arrange
            var command = new DeletePermissionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _mockRepository.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidId_CallsDeleteAsync()
        {
            // Arrange
            var permissionId = 5;
            var command = new DeletePermissionCommand(permissionId);

            _mockRepository
                .Setup(r => r.DeleteAsync(permissionId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(permissionId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Handle_WithDifferentIds_CallsDeleteAsyncWithCorrectId(int permissionId)
        {
            // Arrange
            var command = new DeletePermissionCommand(permissionId);

            _mockRepository
                .Setup(r => r.DeleteAsync(permissionId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(permissionId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteCompletes_ReturnsSuccessWithTrue()
        {
            // Arrange
            var command = new DeletePermissionCommand(1);

            _mockRepository
                .Setup(r => r.DeleteAsync(command.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
