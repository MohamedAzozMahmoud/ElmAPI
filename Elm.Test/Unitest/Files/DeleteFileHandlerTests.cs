using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Files.Handlers;
using Moq;
using FilesEntity = Elm.Domain.Entities.Files;

namespace Elm.Test.Unitest.Files
{
    public class DeleteFileHandlerTests
    {
        private readonly Mock<IGenericRepository<FilesEntity>> _mockFilesRepository;
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly DeleteFileHandler _handler;

        public DeleteFileHandlerTests()
        {
            _mockFilesRepository = new Mock<IGenericRepository<FilesEntity>>();
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new DeleteFileHandler(_mockFilesRepository.Object, _mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteFileCommand(99);

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((FilesEntity)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("File not found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenStorageDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteFileCommand(1);
            var file = new FilesEntity
            {
                Id = 1,
                Name = "test.pdf",
                StorageName = "stored_test.pdf"
            };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.DeleteFile(file.StorageName, "Files"))
                .ReturnsAsync(Result<bool>.Failure("Storage deletion failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete file from storage.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenFileExists_CallsGetByIdAsync()
        {
            // Arrange
            var fileId = 5;
            var command = new DeleteFileCommand(fileId);
            var file = new FilesEntity { Id = fileId, StorageName = "test.pdf" };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.DeleteFile(file.StorageName, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _mockFilesRepository
                .Setup(r => r.DeleteAsync(fileId))
                .ReturnsAsync(false); // Note: Handler logic seems inverted - returns success when DeleteAsync returns false

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFilesRepository.Verify(r => r.GetByIdAsync(fileId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenStorageDeleteSucceeds_CallsDeleteAsync()
        {
            // Arrange
            var command = new DeleteFileCommand(1);
            var file = new FilesEntity
            {
                Id = 1,
                Name = "test.pdf",
                StorageName = "stored_test.pdf"
            };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.DeleteFile(file.StorageName, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _mockFilesRepository
                .Setup(r => r.DeleteAsync(file.Id))
                .ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFilesRepository.Verify(r => r.DeleteAsync(file.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_DoesNotCallDeleteFile()
        {
            // Arrange
            var command = new DeleteFileCommand(99);

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((FilesEntity)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStorageDeleteFails_DoesNotCallRepositoryDeleteAsync()
        {
            // Arrange
            var command = new DeleteFileCommand(1);
            var file = new FilesEntity { Id = 1, StorageName = "test.pdf" };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.DeleteFile(file.StorageName, "Files"))
                .ReturnsAsync(Result<bool>.Failure("Failed"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFilesRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UsesFilesFolder()
        {
            // Arrange
            var command = new DeleteFileCommand(1);
            var file = new FilesEntity { Id = 1, StorageName = "test.pdf" };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.DeleteFile(file.StorageName, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _mockFilesRepository
                .Setup(r => r.DeleteAsync(file.Id))
                .ReturnsAsync(false);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteFile(file.StorageName, "Files"), Times.Once);
        }
    }
}
