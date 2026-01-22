using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Files.Queries;
using Elm.Application.Features.Files.Handlers;
using Moq;

namespace Elm.Test.Unitest.Files
{
    public class GetAllFilesByCurriculumIdHandlerTests
    {
        private readonly Mock<IFileStorageService> _mockFileStorage;
        private readonly GetAllFilesByCurriculumIdHandler _handler;

        public GetAllFilesByCurriculumIdHandlerTests()
        {
            _mockFileStorage = new Mock<IFileStorageService>();
            _handler = new GetAllFilesByCurriculumIdHandler(_mockFileStorage.Object);
        }

        [Fact]
        public async Task Handle_WhenFilesExist_ReturnsSuccessWithFiles()
        {
            // Arrange
            var curriculumId = 1;
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);
            var files = new List<FileView>
            {
                new FileView { Id = 1, Name = "file1.pdf", StorageName = "stored_file1.pdf" },
                new FileView { Id = 2, Name = "file2.docx", StorageName = "stored_file2.docx" }
            };

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(files));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task Handle_WhenNoFilesExist_ReturnsEmptyList()
        {
            // Arrange
            var curriculumId = 99;
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);
            var emptyFiles = new List<FileView>();

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(emptyFiles));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WhenServiceFails_ReturnsFailureResult()
        {
            // Arrange
            var curriculumId = 1;
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Failure("Failed to retrieve files"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to retrieve files", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidCurriculumId_CallsGetAllFilesByCurriculumIdAsync()
        {
            // Arrange
            var curriculumId = 5;
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(new List<FileView>()));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_UsesFilesFolder()
        {
            // Arrange
            var query = new GetAllFilesByCurriculumIdQuery(1);

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(It.IsAny<int>(), "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(new List<FileView>()));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.GetAllFilesByCurriculumIdAsync(It.IsAny<int>(), "Files"), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Handle_WithDifferentCurriculumIds_CallsServiceWithCorrectId(int curriculumId)
        {
            // Arrange
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(new List<FileView>()));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockFileStorage.Verify(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsFilesWithCorrectData()
        {
            // Arrange
            var curriculumId = 1;
            var query = new GetAllFilesByCurriculumIdQuery(curriculumId);
            var files = new List<FileView>
            {
                new FileView
                {
                    Id = 1,
                    Name = "Assignment.pdf",
                    StorageName = "stored_assignment.pdf",
                    DoctorRatedName = "Dr. Smith",
                    comment = "Good work",
                    RatedAt = DateTime.UtcNow
                }
            };

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(curriculumId, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(files));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data!);
            var file = result.Data.First();
            Assert.Equal("Assignment.pdf", file.Name);
            Assert.Equal("Dr. Smith", file.DoctorRatedName);
            Assert.Equal("Good work", file.comment);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ReturnsListOfFileView()
        {
            // Arrange
            var query = new GetAllFilesByCurriculumIdQuery(1);
            var files = new List<FileView>
            {
                new FileView { Id = 1, Name = "file1.pdf" }
            };

            _mockFileStorage
                .Setup(f => f.GetAllFilesByCurriculumIdAsync(1, "Files"))
                .ReturnsAsync(Result<List<FileView>>.Success(files));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<List<FileView>>(result.Data);
        }
    }
}
