using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Images.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Images.Handlers;
using Moq;
using CollegeEntity = Elm.Domain.Entities.College;

namespace Elm.Test.Unitest.Images
{
    public class DeleteCollegeImageHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly Mock<IGenericRepository<CollegeEntity>> _mockCollegeRepository;
        private readonly DeleteCollegeImageHandler _handler;

        public DeleteCollegeImageHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _mockCollegeRepository = new Mock<IGenericRepository<CollegeEntity>>();
            _handler = new DeleteCollegeImageHandler(_mockFileStorage.Object, _mockCollegeRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteSucceeds_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteCollegeImageCommand(1, 10);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(command.collegeId, command.id, "Images"))
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
            var command = new DeleteCollegeImageCommand(1, 10);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(command.collegeId, command.id, "Images"))
                .ReturnsAsync(Result<bool>.Failure("Delete failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Delete failed", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidParameters_CallsDeleteCollegeImageAsync()
        {
            // Arrange
            var collegeId = 5;
            var imageId = 15;
            var command = new DeleteCollegeImageCommand(collegeId, imageId);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imageId, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(collegeId, imageId, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesImagesFolder()
        {
            // Arrange
            var command = new DeleteCollegeImageCommand(1, 1);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(It.IsAny<int>(), It.IsAny<int>(), "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(It.IsAny<int>(), It.IsAny<int>(), "Images"), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 10)]
        [InlineData(100, 200)]
        public async Task Handle_WithDifferentIds_PassesCorrectParameters(int collegeId, int imageId)
        {
            // Arrange
            var command = new DeleteCollegeImageCommand(collegeId, imageId);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imageId, "Images"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(collegeId, imageId, "Images"), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsResultFromFileStorageService()
        {
            // Arrange
            var command = new DeleteCollegeImageCommand(1, 1);
            var expectedResult = Result<bool>.Success(true);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(command.collegeId, command.id, "Images"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.Equal(expectedResult.Data, result.Data);
        }
    }
}
