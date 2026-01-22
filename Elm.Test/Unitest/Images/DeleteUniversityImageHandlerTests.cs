using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Images.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest.Images
{
    public class DeleteUniversityImageHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly Mock<IGenericRepository<University>> _mockUniversityRepository;
        private readonly DeleteUniversityImageHandler _handler;

        public DeleteUniversityImageHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _mockUniversityRepository = new Mock<IGenericRepository<University>>();
            _handler = new DeleteUniversityImageHandler(_mockFileStorage.Object, _mockUniversityRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteUniversityImageCommand(1, 10);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(command.universityId, command.id, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenDeleteFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new DeleteUniversityImageCommand(1, 10);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(command.universityId, command.id, "Images"))
                .ReturnsAsync(Result<bool>.Failure("Delete failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Delete failed", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidParameters_CallsDeleteUniversityImageAsync()
        {
            // Arrange
            var universityId = 5;
            var imageId = 15;
            var command = new DeleteUniversityImageCommand(universityId, imageId);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(universityId, imageId, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteUniversityImageAsync(universityId, imageId, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesImagesFolder()
        {
            // Arrange
            var command = new DeleteUniversityImageCommand(1, 1);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(It.IsAny<int>(), It.IsAny<int>(), "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteUniversityImageAsync(It.IsAny<int>(), It.IsAny<int>(), "Images"), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        [InlineData(100, 200)]
        public async Task Handle_WithDifferentIds_PassesCorrectParameters(int universityId, int imageId)
        {
            // Arrange
            var command = new DeleteUniversityImageCommand(universityId, imageId);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(universityId, imageId, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteUniversityImageAsync(universityId, imageId, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsResultFromFileStorageService()
        {
            // Arrange
            var command = new DeleteUniversityImageCommand(1, 1);
            var expectedResult = Result<bool>.Success(true);

            _mockFileStorage
                .Setup(f => f.DeleteUniversityImageAsync(command.universityId, command.id, "Images"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.Equal(expectedResult.Data, result.Data);
        }
    }
}
