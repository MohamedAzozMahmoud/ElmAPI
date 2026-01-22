using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.College.Handlers;
using Moq;

namespace Elm.Test.Unitest.College
{
    public class DeleteCollegeHandlerTests
    {
        private readonly Mock<ICollegeRepository> _mockRepository;
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly DeleteCollegeHandler _handler;

        public DeleteCollegeHandlerTests()
        {
            _mockRepository = new Mock<ICollegeRepository>();
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new DeleteCollegeHandler(_mockRepository.Object, _mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenDeleteImageSucceeds_ReturnsSuccessResult()
        {
            // Arrange
            var collegeId = 1;
            var imgId = 10;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = imgId
            };
            var deleteResult = Result<bool>.Success(true);

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imgId, "colleges"))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _mockRepository.Verify(r => r.GetByIdAsync(collegeId), Times.Once);
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(collegeId, imgId, "colleges"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteImageFails_ReturnsFailureResult()
        {
            // Arrange
            var collegeId = 1;
            var imgId = 10;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = imgId
            };
            var deleteResult = Result<bool>.Failure("Failed to delete image", 500);

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imgId, "colleges"))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete image", result.Message);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Handle_CallsRepositoryGetByIdAsync_WithCorrectId()
        {
            // Arrange
            var collegeId = 42;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = 5
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsDeleteCollegeImageAsync_WithCorrectParameters()
        {
            // Arrange
            var collegeId = 1;
            var imgId = 15;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = imgId
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imgId, "colleges"))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(1, 15, "colleges"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCollegeHasNoImage_CallsDeleteWithNullableImgId()
        {
            // Arrange
            var collegeId = 1;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = null  // القيمة فارغة
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, 0, "colleges"))  // متوقع 0 عندما ImgId = null
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);  // ✅ تأكد من نجاح العملية
            _mockFileStorage.Verify(f => f.DeleteCollegeImageAsync(collegeId, 0, "colleges"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeleteImageReturnsSuccess_ReturnsDeleteImageResult()
        {
            // Arrange
            var collegeId = 1;
            var imgId = 10;
            var command = new DeleteCollegeCommand(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = imgId
            };
            var expectedResult = Result<bool>.Success(true);

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockFileStorage
                .Setup(f => f.DeleteCollegeImageAsync(collegeId, imgId, "colleges"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult.IsSuccess, result.IsSuccess);
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        }
    }
}
