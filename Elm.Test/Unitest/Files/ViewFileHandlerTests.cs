using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Queries;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Application.Features.Files.Handlers;
using Moq;

namespace Elm.Test.Unitest.Files
{
    public class ViewFileHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly ViewFileHandler _handler;

        public ViewFileHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new ViewFileHandler(_mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenFileExists_ReturnsSuccessWithImageDto()
        {
            // Arrange
            var fileName = "image.png";
            var command = new ViewFileCommand(fileName);
            var expectedResponse = new ImageDto
            {
                Content = new byte[] { 0x89, 0x50, 0x4E, 0x47 }, // PNG magic bytes
                ContentType = "image/png"
            };

            _mockFileStorage
                .Setup(f => f.GetFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("image/png", result.Data.ContentType);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_ReturnsFailureResult()
        {
            // Arrange
            var fileName = "nonexistent.png";
            var command = new ViewFileCommand(fileName);

            _mockFileStorage
                .Setup(f => f.GetFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<ImageDto>.Failure("File not found", 404));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("File not found", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidFileName_CallsGetFileAsync()
        {
            // Arrange
            var fileName = "document.pdf";
            var command = new ViewFileCommand(fileName);

            _mockFileStorage
                .Setup(f => f.GetFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<ImageDto>.Success(new ImageDto()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.GetFileAsync(fileName, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesFilesFolder()
        {
            // Arrange
            var command = new ViewFileCommand("test.pdf");

            _mockFileStorage
                .Setup(f => f.GetFileAsync(It.IsAny<string>(), "Files"))
                .ReturnsAsync(Result<ImageDto>.Success(new ImageDto()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.GetFileAsync(It.IsAny<string>(), "Files"), Times.Once);
        }

        [Theory]
        [InlineData("image.png", "image/png")]
        [InlineData("document.pdf", "application/pdf")]
        [InlineData("photo.jpg", "image/jpeg")]
        public async Task Handle_WithDifferentFileTypes_ReturnsCorrectContentType(string fileName, string contentType)
        {
            // Arrange
            var command = new ViewFileCommand(fileName);
            var expectedResponse = new ImageDto
            {
                Content = new byte[] { 1, 2, 3 },
                ContentType = contentType
            };

            _mockFileStorage
                .Setup(f => f.GetFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(contentType, result.Data!.ContentType);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsFileContent()
        {
            // Arrange
            var fileName = "test.pdf";
            var command = new ViewFileCommand(fileName);
            var fileContent = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF magic bytes
            var expectedResponse = new ImageDto
            {
                Content = fileContent,
                ContentType = "application/pdf"
            };

            _mockFileStorage
                .Setup(f => f.GetFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<ImageDto>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(fileContent, result.Data!.Content);
        }
    }
}
