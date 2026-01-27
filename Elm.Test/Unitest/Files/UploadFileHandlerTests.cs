using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Features.Files.Handlers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Elm.Test.Unitest.Files
{
    public class UploadFileHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly UploadFileHandler _handler;

        public UploadFileHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new UploadFileHandler(_mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenFileUploadedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.pdf");
            mockFile.Setup(f => f.Length).Returns(1024);

            var command = new UploadFileCommand(1, 10, "Test description", mockFile.Object);
            var expectedFileName = "stored_test.pdf";

            _mockFileStorage
                .Setup(f => f.UploadFileAsync(command.curriculumId, command.uploadedById, command.Description, command.FormFile, "Files"))
                .ReturnsAsync(Result<string>.Success(expectedFileName));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedFileName, result.Data);
        }

        [Fact]
        public async Task Handle_WhenFileUploadFails_ReturnsFailureResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.pdf");

            var command = new UploadFileCommand(1, 10, "Test description", mockFile.Object);

            _mockFileStorage
                .Setup(f => f.UploadFileAsync(command.curriculumId, command.uploadedById, command.Description, command.FormFile, "Files"))
                .ReturnsAsync(Result<string>.Failure("Upload failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Upload failed", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidParameters_CallsUploadFileAsync()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var curriculumId = 5;
            var uploadedById = 15;
            var command = new UploadFileCommand(curriculumId, uploadedById, "Test description", mockFile.Object);

            _mockFileStorage
                .Setup(f => f.UploadFileAsync(curriculumId, uploadedById, "Test description", mockFile.Object, "Files"))
                .ReturnsAsync(Result<string>.Success("file.pdf"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.UploadFileAsync(curriculumId, uploadedById, "Test description", mockFile.Object, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesFilesFolder()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var command = new UploadFileCommand(1, 1, "Test description", mockFile.Object);

            _mockFileStorage
                .Setup(f => f.UploadFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IFormFile>(), "Files"))
                .ReturnsAsync(Result<string>.Success("file.pdf"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.UploadFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IFormFile>(), "Files"), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        [InlineData(100, 200)]
        public async Task Handle_WithDifferentIds_PassesCorrectParameters(int curriculumId, int uploadedById)
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var command = new UploadFileCommand(curriculumId, uploadedById, "Test description", mockFile.Object);

            _mockFileStorage
                .Setup(f => f.UploadFileAsync(curriculumId, uploadedById, "Test description", mockFile.Object, "Files"))
                .ReturnsAsync(Result<string>.Success("file.pdf"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.UploadFileAsync(curriculumId, uploadedById, "Test description", mockFile.Object, "Files"), Times.Once);
        }
    }
}
