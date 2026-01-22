using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Files.Queries;
using Elm.Application.Features.Files.Handlers;
using Moq;

namespace Elm.Test.Unitest.Files
{
    public class DownloadFileHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly DownloadFileHandler _handler;

        public DownloadFileHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new DownloadFileHandler(_mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenFileExists_ReturnsSuccessWithFileResponse()
        {
            // Arrange
            var fileName = "test_document.pdf";
            var command = new DownloadFileCommand(fileName);
            var expectedResponse = new FileResponse
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = "application/pdf",
                FileName = fileName
            };

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<FileResponse>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(fileName, result.Data.FileName);
            Assert.Equal("application/pdf", result.Data.ContentType);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_ReturnsFailureResult()
        {
            // Arrange
            var fileName = "nonexistent.pdf";
            var command = new DownloadFileCommand(fileName);

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<FileResponse>.Failure("File not found", 404));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("File not found", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidFileName_CallsDownloadFileAsync()
        {
            // Arrange
            var fileName = "document.docx";
            var command = new DownloadFileCommand(fileName);

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<FileResponse>.Success(new FileResponse()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DownloadFileAsync(fileName, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesFilesFolder()
        {
            // Arrange
            var command = new DownloadFileCommand("test.pdf");

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(It.IsAny<string>(), "Files"))
                .ReturnsAsync(Result<FileResponse>.Success(new FileResponse()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DownloadFileAsync(It.IsAny<string>(), "Files"), Times.Once);
        }

        [Theory]
        [InlineData("document.pdf")]
        [InlineData("image.png")]
        [InlineData("spreadsheet.xlsx")]
        [InlineData("presentation.pptx")]
        public async Task Handle_WithDifferentFileNames_CallsDownloadFileAsyncWithCorrectFileName(string fileName)
        {
            // Arrange
            var command = new DownloadFileCommand(fileName);

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<FileResponse>.Success(new FileResponse { FileName = fileName }));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DownloadFileAsync(fileName, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsFileContent()
        {
            // Arrange
            var fileName = "test.pdf";
            var command = new DownloadFileCommand(fileName);
            var fileContent = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF magic bytes
            var expectedResponse = new FileResponse
            {
                Content = fileContent,
                ContentType = "application/pdf",
                FileName = fileName
            };

            _mockFileStorage
                .Setup(f => f.DownloadFileAsync(fileName, "Files"))
                .ReturnsAsync(Result<FileResponse>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(fileContent, result.Data!.Content);
        }
    }
}
