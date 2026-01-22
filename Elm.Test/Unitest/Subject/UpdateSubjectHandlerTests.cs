using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Subject.Handlers;
using Moq;

namespace Elm.Test.Unitest.Subject
{
    public class UpdateSubjectHandlerTests
    {
        private readonly Mock<ISubjectRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateSubjectHandler _handler;

        public UpdateSubjectHandlerTests()
        {
            _repositoryMock = new Mock<ISubjectRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateSubjectHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenSubjectExists_ReturnsSuccessWithUpdatedDto()
        {
            // Arrange
            var subjectId = 1;
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Old Name",
                Code = "OLD001"
            };

            var command = new UpdateSubjectCommand(subjectId, "New Name", "NEW001");

            var updatedSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "New Name",
                Code = "NEW001"
            };

            var expectedDto = new SubjectDto
            {
                Id = subjectId.ToString(),
                Name = "New Name",
                Code = "NEW001"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(updatedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(updatedSubject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.Code, result.Data.Code);
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSubjectNotFound_ReturnsFailure()
        {
            // Arrange
            var subjectId = 999;
            var command = new UpdateSubjectCommand(subjectId, "New Name", "NEW001");

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync((Elm.Domain.Entities.Subject?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Subject not found.", result.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Never);
            _mapperMock.Verify(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUpdateFails_ReturnsFailure()
        {
            // Arrange
            var subjectId = 1;
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Old Name",
                Code = "OLD001"
            };

            var command = new UpdateSubjectCommand(subjectId, "New Name", "NEW001");

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync((Elm.Domain.Entities.Subject?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to update subject.", result.Message);
            _mapperMock.Verify(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdatesNameCorrectly()
        {
            // Arrange
            var subjectId = 1;
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Original Name",
                Code = "CODE001"
            };

            var command = new UpdateSubjectCommand(subjectId, "Updated Name", "CODE001");

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync((Elm.Domain.Entities.Subject s) => s);

            _mapperMock.Setup(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()))
                .Returns(new SubjectDto { Id = subjectId.ToString(), Name = "Updated Name", Code = "CODE001" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Subject>(s =>
                s.Name == "Updated Name")), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdatesCodeCorrectly()
        {
            // Arrange
            var subjectId = 1;
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Test Subject",
                Code = "OLD001"
            };

            var command = new UpdateSubjectCommand(subjectId, "Test Subject", "NEW001");

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync((Elm.Domain.Entities.Subject s) => s);

            _mapperMock.Setup(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()))
                .Returns(new SubjectDto { Id = subjectId.ToString(), Name = "Test Subject", Code = "NEW001" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Subject>(s =>
                s.Code == "NEW001")), Times.Once);
        }

        [Theory]
        [InlineData(1, "Subject A", "SUBA001")]
        [InlineData(2, "Subject B", "SUBB001")]
        [InlineData(3, "Subject C", "SUBC001")]
        public async Task Handle_WithVariousInputs_UpdatesCorrectly(int id, string name, string code)
        {
            // Arrange
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = id,
                Name = "Old Name",
                Code = "OLD001"
            };

            var command = new UpdateSubjectCommand(id, name, code);

            var updatedSubject = new Elm.Domain.Entities.Subject
            {
                Id = id,
                Name = name,
                Code = code
            };

            var expectedDto = new SubjectDto
            {
                Id = id.ToString(),
                Name = name,
                Code = code
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(updatedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(updatedSubject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(code, result.Data?.Code);
        }

        [Fact]
        public async Task Handle_RepositoryGetByIdCalledFirst()
        {
            // Arrange
            var subjectId = 1;
            var existingSubject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Test",
                Code = "TEST"
            };

            var command = new UpdateSubjectCommand(subjectId, "New Name", "NEW001");

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(existingSubject);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(existingSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()))
                .Returns(new SubjectDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
        }
    }
}
