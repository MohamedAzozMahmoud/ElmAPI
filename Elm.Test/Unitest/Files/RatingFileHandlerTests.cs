using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Files.Handlers;
using Elm.Domain.Enums;
using Moq;
using FilesEntity = Elm.Domain.Entities.Files;

namespace Elm.Test.Unitest.Files
{
    public class RatingFileHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly Mock<IGenericRepository<FilesEntity>> _mockFilesRepository;
        private readonly RatingFileHandler _handler;

        public RatingFileHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _mockFilesRepository = new Mock<IGenericRepository<FilesEntity>>();
            _handler = new RatingFileHandler(_mockFileStorage.Object, _mockFilesRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new RatingFileCommand(99, 1, "Comment", DoctorRating.Good);

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync((FilesEntity)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("File not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenFileExists_RatesFileSuccessfully()
        {
            // Arrange
            var command = new RatingFileCommand(1, 5, "Great work!", DoctorRating.Excellent);
            var file = new FilesEntity
            {
                Id = 1,
                Name = "test.pdf",
                CurriculumId = 10
            };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, command.rating, command.comment))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenFileExists_CallsRatingFileAsync()
        {
            // Arrange
            var command = new RatingFileCommand(1, 5, "Good", DoctorRating.Good);
            var file = new FilesEntity
            {
                Id = 1,
                CurriculumId = 10
            };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, command.rating, command.comment))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, command.rating, command.comment), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenFileNotFound_DoesNotCallRatingFileAsync()
        {
            // Arrange
            var command = new RatingFileCommand(99, 1, "Comment", DoctorRating.Good);

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync((FilesEntity)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.RatingFileAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DoctorRating>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(DoctorRating.Poor)]
        [InlineData(DoctorRating.Acceptable)]
        [InlineData(DoctorRating.Good)]
        [InlineData(DoctorRating.Excellent)]
        public async Task Handle_WithDifferentRatings_PassesCorrectRating(DoctorRating rating)
        {
            // Arrange
            var command = new RatingFileCommand(1, 5, "Comment", rating);
            var file = new FilesEntity { Id = 1, CurriculumId = 10 };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, rating, command.comment))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, rating, command.comment), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRatingFails_ReturnsFailureResult()
        {
            // Arrange
            var command = new RatingFileCommand(1, 5, "Comment", DoctorRating.Good);
            var file = new FilesEntity { Id = 1, CurriculumId = 10 };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.RatingFileAsync(file.CurriculumId, command.ratedByDoctorId, command.fileId, command.rating, command.comment))
                .ReturnsAsync(Result<bool>.Failure("Rating failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Rating failed", result.Message);
        }

        [Fact]
        public async Task Handle_UsesFileCurriculumId()
        {
            // Arrange
            var command = new RatingFileCommand(1, 5, "Comment", DoctorRating.Good);
            var file = new FilesEntity { Id = 1, CurriculumId = 25 };

            _mockFilesRepository
                .Setup(r => r.GetByIdAsync(command.fileId))
                .ReturnsAsync(file);

            _mockFileStorage
                .Setup(f => f.RatingFileAsync(25, command.ratedByDoctorId, command.fileId, command.rating, command.comment))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.RatingFileAsync(25, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DoctorRating>(), It.IsAny<string>()), Times.Once);
        }
    }
}
