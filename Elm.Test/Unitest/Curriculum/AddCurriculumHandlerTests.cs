using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.Curriculum
{
    public class AddCurriculumHandlerTests
    {
        private readonly Mock<ICurriculumRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddCurriculumHandler _handler;

        public AddCurriculumHandlerTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddCurriculumHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsSuccessWithCurriculumDto()
        {
            // Arrange
            var command = new AddCurriculumCommand(1, 2, 3, 4);

            var addedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = 1,
                SubjectId = 1,
                YearId = 2,
                DepartmentId = 3,
                DoctorId = 4
            };

            var expectedDto = new CurriculumDto
            {
                Id = 1,
                SubjectId = 1,
                YearId = 2,
                DepartmentId = 3,
                DoctorId = 4
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(addedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(addedCurriculum))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.SubjectId, result.Data.SubjectId);
            Assert.Equal(expectedDto.YearId, result.Data.YearId);
            Assert.Equal(expectedDto.DepartmentId, result.Data.DepartmentId);
            Assert.Equal(expectedDto.DoctorId, result.Data.DoctorId);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Elm.Domain.Entities.Curriculum>(c =>
                c.SubjectId == command.SubjectId &&
                c.YearId == command.YearId &&
                c.DepartmentId == command.DepartmentId &&
                c.DoctorId == command.DoctorId)), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(5, 10, 15, 20)]
        [InlineData(100, 200, 300, 400)]
        public async Task Handle_WithVariousInputs_CreatesCorrectCurriculum(int subjectId, int yearId, int departmentId, int doctorId)
        {
            // Arrange
            var command = new AddCurriculumCommand(subjectId, yearId, departmentId, doctorId);

            var addedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = 1,
                SubjectId = subjectId,
                YearId = yearId,
                DepartmentId = departmentId,
                DoctorId = doctorId
            };

            var expectedDto = new CurriculumDto
            {
                Id = 1,
                SubjectId = subjectId,
                YearId = yearId,
                DepartmentId = departmentId,
                DoctorId = doctorId
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(addedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(addedCurriculum))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subjectId, result.Data?.SubjectId);
            Assert.Equal(yearId, result.Data?.YearId);
            Assert.Equal(departmentId, result.Data?.DepartmentId);
            Assert.Equal(doctorId, result.Data?.DoctorId);
        }

        [Fact]
        public async Task Handle_CallsRepositoryWithCorrectEntity()
        {
            // Arrange
            var command = new AddCurriculumCommand(10, 20, 30, 40);
            Elm.Domain.Entities.Curriculum? capturedCurriculum = null;

            var addedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = 1,
                SubjectId = 10,
                YearId = 20,
                DepartmentId = 30,
                DoctorId = 40
            };

            var expectedDto = new CurriculumDto
            {
                Id = 1,
                SubjectId = 10,
                YearId = 20,
                DepartmentId = 30,
                DoctorId = 40
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .Callback<Elm.Domain.Entities.Curriculum>(c => capturedCurriculum = c)
                .ReturnsAsync(addedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(addedCurriculum))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedCurriculum);
            Assert.Equal(command.SubjectId, capturedCurriculum.SubjectId);
            Assert.Equal(command.YearId, capturedCurriculum.YearId);
            Assert.Equal(command.DepartmentId, capturedCurriculum.DepartmentId);
            Assert.Equal(command.DoctorId, capturedCurriculum.DoctorId);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var command = new AddCurriculumCommand(1, 2, 3, 4);

            var addedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = 5,
                SubjectId = 1,
                YearId = 2,
                DepartmentId = 3,
                DoctorId = 4
            };

            var expectedDto = new CurriculumDto
            {
                Id = 5,
                SubjectId = 1,
                YearId = 2,
                DepartmentId = 3,
                DoctorId = 4
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(addedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(addedCurriculum))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<CurriculumDto>(addedCurriculum), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var command = new AddCurriculumCommand(1, 2, 3, 4);

            var addedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = 1,
                SubjectId = 1,
                YearId = 2,
                DepartmentId = 3,
                DoctorId = 4
            };

            var expectedDto = new CurriculumDto();

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(addedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()), Times.Once);
        }
    }
}
