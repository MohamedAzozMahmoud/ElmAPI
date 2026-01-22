using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Application.Contracts.Features.Images.Queries;
using Elm.Application.Features.Images.Handlers;
using Moq;

namespace Elm.Test.Unitest.Images
{
    public class ShowImageFromUrlHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly showImageFromUrlHandler _handler;

        public ShowImageFromUrlHandlerTests()
        {
            _mockFileStorageService = new Mock<IFileStorageService>();
            _handler = new showImageFromUrlHandler(_mockFileStorageService.Object);
        }

        [Fact]
        public async Task Handle_WhenImageExists_ReturnsSuccessWithImageDto()
        {
            // Arrange
            var fileName = "test_image.png";
            var command = new showImageFromUrlCommand(fileName);
            var expectedImage = new ImageDto
            {
                Content = new byte[] { 0x89, 0x50, 0x4E, 0x47 }, // PNG magic bytes
                ContentType = "image/png"
            };

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedImage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("image/png", result.Data.ContentType);
        }

        [Fact]
        public async Task Handle_WhenImageNotFound_ReturnsFailureResult()
        {
            // Arrange
            var fileName = "nonexistent.png";
            var command = new showImageFromUrlCommand(fileName);

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Failure("File not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Image not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenServiceReturnsNullData_ReturnsFailureResult()
        {
            // Arrange
            var fileName = "test.png";
            var command = new showImageFromUrlCommand(fileName);

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(null!));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Image not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WithValidFileName_CallsGetFileAsync()
        {
            // Arrange
            var fileName = "image.jpg";
            var command = new showImageFromUrlCommand(fileName);

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(new ImageDto()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.GetFileAsync(fileName, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesImagesFolder()
        {
            // Arrange
            var command = new showImageFromUrlCommand("test.png");

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(It.IsAny<string>(), "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(new ImageDto()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.GetFileAsync(It.IsAny<string>(), "Images"), Times.Once);
        }

        [Theory]
        [InlineData("image.png", "image/png")]
        [InlineData("photo.jpg", "image/jpeg")]
        [InlineData("logo.gif", "image/gif")]
        [InlineData("banner.webp", "image/webp")]
        public async Task Handle_WithDifferentImageTypes_ReturnsCorrectContentType(string fileName, string contentType)
        {
            // Arrange
            var command = new showImageFromUrlCommand(fileName);
            var expectedImage = new ImageDto
            {
                Content = new byte[] { 1, 2, 3 },
                ContentType = contentType
            };

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedImage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(contentType, result.Data!.ContentType);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsImageContent()
        {
            // Arrange
            var fileName = "test.png";
            var command = new showImageFromUrlCommand(fileName);
            var imageContent = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            var expectedImage = new ImageDto
            {
                Content = imageContent,
                ContentType = "image/png"
            };

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedImage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(imageContent, result.Data!.Content);
        }

        [Theory]
        [InlineData("image1.png")]
        [InlineData("folder/image2.jpg")]
        [InlineData("stored_guid_123456.png")]
        public async Task Handle_WithDifferentFileNames_CallsGetFileAsyncWithCorrectFileName(string fileName)
        {
            // Arrange
            var command = new showImageFromUrlCommand(fileName);

            _mockFileStorageService
                .Setup(f => f.GetFileAsync(fileName, "Images"))
                .ReturnsAsync(Result<ImageDto>.Success(new ImageDto()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorageService.Verify(f => f.GetFileAsync(fileName, "Images"), Times.Once);
        }
    }
}
