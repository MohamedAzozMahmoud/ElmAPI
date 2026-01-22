using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.Curriculum
{
    public class DeleteCurriculumHandlerTests
    {
        private readonly Mock<ICurriculumRepository> _repositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly DeleteCurriculumHandler _handler;

        public DeleteCurriculumHandlerTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _fileStorageMock = new Mock<IFileStorageService>();
            _handler = new DeleteCurriculumHandler(_repositoryMock.Object, _fileStorageMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCurriculumExistsAndDeleteSucceeds_ReturnsSuccessWithTrue()
        {
            // Arrange
            var curriculumId = 1;
            var command = new DeleteCurriculumCommand(curriculumId);

            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _fileStorageMock.Setup(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _repositoryMock.Setup(r => r.DeleteAsync(curriculumId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _fileStorageMock.Verify(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurriculumNotFound_ReturnsFailureWith404()
        {
            // Arrange
            var curriculumId = 999;
            var command = new DeleteCurriculumCommand(curriculumId);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync((Elm.Domain.Entities.Curriculum?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Curriculum not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _fileStorageMock.Verify(f => f.DeleteAllFilesByCurriculumId(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenFileDeleteFails_ReturnsFailure()
        {
            // Arrange
            var curriculumId = 1;
            var command = new DeleteCurriculumCommand(curriculumId);

            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _fileStorageMock.Setup(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"))
                .ReturnsAsync(Result<bool>.Failure("File deletion failed"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to delete associated files.", result.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _fileStorageMock.Verify(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int curriculumId)
        {
            // Arrange
            var command = new DeleteCurriculumCommand(curriculumId);

            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _fileStorageMock.Setup(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _repositoryMock.Setup(r => r.DeleteAsync(curriculumId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAsync(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_DeletesFilesBeforeCurriculum()
        {
            // Arrange
            var curriculumId = 1;
            var command = new DeleteCurriculumCommand(curriculumId);
            var callOrder = new List<string>();

            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _fileStorageMock.Setup(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"))
                .Callback(() => callOrder.Add("DeleteFiles"))
                .ReturnsAsync(Result<bool>.Success(true));

            _repositoryMock.Setup(r => r.DeleteAsync(curriculumId))
                .Callback(() => callOrder.Add("DeleteCurriculum"))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(2, callOrder.Count);
            Assert.Equal("DeleteFiles", callOrder[0]);
            Assert.Equal("DeleteCurriculum", callOrder[1]);
        }

        [Fact]
        public async Task Handle_UsesCorrectFolderNameForFileStorage()
        {
            // Arrange
            var curriculumId = 1;
            var command = new DeleteCurriculumCommand(curriculumId);

            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _fileStorageMock.Setup(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"))
                .ReturnsAsync(Result<bool>.Success(true));

            _repositoryMock.Setup(r => r.DeleteAsync(curriculumId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _fileStorageMock.Verify(f => f.DeleteAllFilesByCurriculumId(curriculumId, "Files"), Times.Once);
        }
    }
}
