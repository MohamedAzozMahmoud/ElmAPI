using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.University.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest
{
    public class DeleteUniversityHandlerTests
    {
        private readonly Mock<IGenericRepository<University>> _mockRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly DeleteUniversityHandler _handler;

        public DeleteUniversityHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<University>>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _handler = new DeleteUniversityHandler(_mockRepository.Object, _mockFileStorageService.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesImageAndReturnsSuccess()
        {
            // Arrange
            var command = new DeleteUniversityCommand(1);
            var university = new University { Id = 1, Name = "Test University", ImgId = 10 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(1, 10, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_ImageDeletionFails_ReturnsFailure()
        {
            // Arrange
            var command = new DeleteUniversityCommand(1);
            var university = new University { Id = 1, Name = "Test University", ImgId = 10 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(1, 10, "Images"))
                .ReturnsAsync(Result<bool>.Failure("Delete failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete image from storage", result.Message);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryGetByIdAsync()
        {
            // Arrange
            var command = new DeleteUniversityCommand(5);
            var university = new University { Id = 5, Name = "Test University", ImgId = 15 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsDeleteUniversityImageAsync()
        {
            // Arrange
            var command = new DeleteUniversityCommand(2);
            var university = new University { Id = 2, Name = "Test University", ImgId = 20 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(2, 20, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.DeleteUniversityImageAsync(2, 20, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_UniversityWithNullImgId_UsesZeroAsImageId()
        {
            // Arrange
            var command = new DeleteUniversityCommand(10);
            var university = new University { Id = 10, Name = "Test University", ImgId = null };

            _mockRepository
                .Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(10, 0, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);  // ✅ أضف هذا للتحقق من النتيجة
            _mockFileStorageService.Verify(f => f.DeleteUniversityImageAsync(10, 0, "Images"), Times.Once);  // ✅ ID = 10
        }

        [Fact]
        public async Task Handle_SuccessfulDeletion_ReturnsStatusCode200()
        {
            // Arrange
            var command = new DeleteUniversityCommand(1);
            var university = new University { Id = 1, Name = "Test University", ImgId = 10 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(1, 10, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Handle_FailedDeletion_ReturnsStatusCode400()
        {
            // Arrange
            var command = new DeleteUniversityCommand(1);
            var university = new University { Id = 1, Name = "Test University", ImgId = 10 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(university);

            _mockFileStorageService
                .Setup(f => f.DeleteUniversityImageAsync(1, 10, "Images"))
                .ReturnsAsync(Result<bool>.Failure("Delete failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(400, result.StatusCode);
        }
    }
}
