using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Images.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Elm.Test.Unitest.Images
{
    public class AddUniversityImageHandlerTests
    {
        private readonly Mock<IGenericRepository<University>> _mockUniversityRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly AddUniversityImageHandler _handler;

        public AddUniversityImageHandlerTests()
        {
            _mockUniversityRepository = new Mock<IGenericRepository<University>>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _handler = new AddUniversityImageHandler(_mockUniversityRepository.Object, _mockFileStorageService.Object);
        }

        [Fact]
        public async Task Handle_WhenImageUploadFails_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddUniversityImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Failure("Upload failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Image upload failed", result.Message);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenImageUploadReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddUniversityImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success(null!));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Image upload failed", result.Message);
        }

        [Fact]
        public async Task Handle_WhenUniversityNotFound_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddUniversityImageCommand(99, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test.png"));

            _mockUniversityRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync((University)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("University not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsSuccessResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("university_image.png");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            var command = new AddUniversityImageCommand(1, mockFile.Object);
            var university = new University { Id = 1, Name = "Test University" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_university_image.png"));

            _mockUniversityRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(university);

            _mockUniversityRepository
                .Setup(r => r.UpdateAsync(university))
                .ReturnsAsync(university);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_SetsUniversityImage()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test_image.jpg");
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            var command = new AddUniversityImageCommand(1, mockFile.Object);
            var university = new University { Id = 1, Name = "Test University" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test_image.jpg"));

            _mockUniversityRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(university);

            _mockUniversityRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync((University u) => u);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(university.Img);
            Assert.Equal("test_image.jpg", university.Img.Name);
            Assert.Equal("image/jpeg", university.Img.ContentType);
            Assert.Equal("stored_test_image.jpg", university.Img.StorageName);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsUpdateAsync()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            var command = new AddUniversityImageCommand(1, mockFile.Object);
            var university = new University { Id = 1, Name = "Test University" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test.png"));

            _mockUniversityRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(university);

            _mockUniversityRepository
                .Setup(r => r.UpdateAsync(university))
                .ReturnsAsync(university);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUniversityRepository.Verify(r => r.UpdateAsync(university), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenImageUploadFails_DoesNotCallGetByIdAsync()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var command = new AddUniversityImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Failure("Upload failed"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUniversityRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UsesImagesFolder()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddUniversityImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Failure("Failed"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.UploadImageAsync(mockFile.Object, "Images"), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public async Task Handle_WithDifferentUniversityIds_CallsGetByIdAsyncWithCorrectId(int universityId)
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            var command = new AddUniversityImageCommand(universityId, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test.png"));

            _mockUniversityRepository
                .Setup(r => r.GetByIdAsync(universityId))
                .ReturnsAsync(new University { Id = universityId, Name = "Test" });

            _mockUniversityRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync((University u) => u);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUniversityRepository.Verify(r => r.GetByIdAsync(universityId), Times.Once);
        }
    }
}
