using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Images.Handlers;
using Microsoft.AspNetCore.Http;
using Moq;
using CollegeEntity = Elm.Domain.Entities.College;
using ImageEntity = Elm.Domain.Entities.Image;

namespace Elm.Test.Unitest.Images
{
    public class AddCollegeImageHandlerTests
    {
        private readonly Mock<ICollegeRepository> _mockCollegeRepository;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly AddCollegeImageHandler _handler;

        public AddCollegeImageHandlerTests()
        {
            _mockCollegeRepository = new Mock<ICollegeRepository>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _handler = new AddCollegeImageHandler(_mockCollegeRepository.Object, _mockFileStorageService.Object);
        }

        [Fact]
        public async Task Handle_WhenImageUploadFails_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddCollegeImageCommand(1, mockFile.Object);

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
            var command = new AddCollegeImageCommand(1, mockFile.Object);

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
        public async Task Handle_WhenCollegeNotFound_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddCollegeImageCommand(99, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test.png"));

            _mockCollegeRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync((CollegeEntity)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("College not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsSuccessResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("college_image.png");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            var command = new AddCollegeImageCommand(1, mockFile.Object);
            var college = new CollegeEntity { Id = 1, Name = "Test College" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_college_image.png"));

            _mockCollegeRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(college);

            _mockCollegeRepository
                .Setup(r => r.UpdateAsync(college))
                .ReturnsAsync(college);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_SetsCollegeImage()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test_image.jpg");
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            var command = new AddCollegeImageCommand(1, mockFile.Object);
            var college = new CollegeEntity { Id = 1, Name = "Test College" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test_image.jpg"));

            _mockCollegeRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(college);

            _mockCollegeRepository
                .Setup(r => r.UpdateAsync(It.IsAny<CollegeEntity>()))
                .ReturnsAsync((CollegeEntity c) => c);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(college.Img);
            Assert.Equal("test_image.jpg", college.Img.Name);
            Assert.Equal("image/jpeg", college.Img.ContentType);
            Assert.Equal("stored_test_image.jpg", college.Img.StorageName);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsUpdateAsync()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.ContentType).Returns("image/png");
            var command = new AddCollegeImageCommand(1, mockFile.Object);
            var college = new CollegeEntity { Id = 1, Name = "Test College" };

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Success("/Images/stored_test.png"));

            _mockCollegeRepository
                .Setup(r => r.GetByIdAsync(command.id))
                .ReturnsAsync(college);

            _mockCollegeRepository
                .Setup(r => r.UpdateAsync(college))
                .ReturnsAsync(college);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockCollegeRepository.Verify(r => r.UpdateAsync(college), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenImageUploadFails_DoesNotCallGetByIdAsync()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var command = new AddCollegeImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Failure("Upload failed"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockCollegeRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UsesImagesFolder()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            var command = new AddCollegeImageCommand(1, mockFile.Object);

            _mockFileStorageService
                .Setup(f => f.UploadImageAsync(mockFile.Object, "Images"))
                .ReturnsAsync(Result<string>.Failure("Failed"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.UploadImageAsync(mockFile.Object, "Images"), Times.Once);
        }
    }
}
