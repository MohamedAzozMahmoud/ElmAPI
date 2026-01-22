using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Subject.Handlers;
using Moq;

namespace Elm.Test.Unitest.Subject
{
    public class AddSubjectHandlerTests
    {
        private readonly Mock<ISubjectRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddSubjectHandler _handler;

        public AddSubjectHandlerTests()
        {
            _repositoryMock = new Mock<ISubjectRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddSubjectHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsSuccessWithSubjectDto()
        {
            // Arrange
            var command = new AddSubjectCommand("Mathematics", "MATH101");

            var addedSubject = new Elm.Domain.Entities.Subject
            {
                Id = 1,
                Name = "Mathematics",
                Code = "MATH101"
            };

            var expectedDto = new SubjectDto
            {
                Id = "1",
                Name = "Mathematics",
                Code = "MATH101"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(addedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(addedSubject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.Code, result.Data.Code);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Elm.Domain.Entities.Subject>(s =>
                s.Name == command.Name &&
                s.Code == command.Code)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ReturnsFailure()
        {
            // Arrange
            var command = new AddSubjectCommand("Test Subject", "TEST001");

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync((Elm.Domain.Entities.Subject?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add subject.", result.Message);
            _mapperMock.Verify(m => m.Map<SubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Never);
        }

        [Theory]
        [InlineData("Physics", "PHY101")]
        [InlineData("Chemistry", "CHEM101")]
        [InlineData("Biology", "BIO101")]
        public async Task Handle_WithVariousInputs_CreatesCorrectSubject(string name, string code)
        {
            // Arrange
            var command = new AddSubjectCommand(name, code);

            var addedSubject = new Elm.Domain.Entities.Subject
            {
                Id = 1,
                Name = name,
                Code = code
            };

            var expectedDto = new SubjectDto
            {
                Id = "1",
                Name = name,
                Code = code
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(addedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(addedSubject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(code, result.Data?.Code);
        }

        [Fact]
        public async Task Handle_CallsRepositoryWithCorrectEntity()
        {
            // Arrange
            var command = new AddSubjectCommand("Test Subject", "TEST001");
            Elm.Domain.Entities.Subject? capturedSubject = null;

            var addedSubject = new Elm.Domain.Entities.Subject
            {
                Id = 1,
                Name = "Test Subject",
                Code = "TEST001"
            };

            var expectedDto = new SubjectDto
            {
                Id = "1",
                Name = "Test Subject",
                Code = "TEST001"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .Callback<Elm.Domain.Entities.Subject>(s => capturedSubject = s)
                .ReturnsAsync(addedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(addedSubject))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedSubject);
            Assert.Equal(command.Name, capturedSubject.Name);
            Assert.Equal(command.Code, capturedSubject.Code);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var command = new AddSubjectCommand("English", "ENG101");

            var addedSubject = new Elm.Domain.Entities.Subject
            {
                Id = 5,
                Name = "English",
                Code = "ENG101"
            };

            var expectedDto = new SubjectDto
            {
                Id = "5",
                Name = "English",
                Code = "ENG101"
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Subject>()))
                .ReturnsAsync(addedSubject);

            _mapperMock.Setup(m => m.Map<SubjectDto>(addedSubject))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<SubjectDto>(addedSubject), Times.Once);
        }
    }
}
