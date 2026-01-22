using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.Curriculum
{
    public class UpdateCurriculumHandlerTests
    {
        private readonly Mock<ICurriculumRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateCurriculumHandler _handler;

        public UpdateCurriculumHandlerTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateCurriculumHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCurriculumExists_ReturnsSuccessWithUpdatedDto()
        {
            // Arrange
            var curriculumId = 1;
            var existingCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var command = new UpdateCurriculumCommand(curriculumId, 10, 20, 30, 40);

            var updatedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 10,
                YearId = 20,
                DepartmentId = 30,
                DoctorId = 40
            };

            var expectedDto = new CurriculumDto
            {
                Id = curriculumId,
                SubjectId = 10,
                YearId = 20,
                DepartmentId = 30,
                DoctorId = 40
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(existingCurriculum);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(updatedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(updatedCurriculum))
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
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurriculumNotFound_ReturnsFailureWith404()
        {
            // Arrange
            var curriculumId = 999;
            var command = new UpdateCurriculumCommand(curriculumId, 10, 20, 30, 40);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync((Elm.Domain.Entities.Curriculum?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Curriculum not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()), Times.Never);
            _mapperMock.Verify(m => m.Map<CurriculumDto>(It.IsAny<Elm.Domain.Entities.Curriculum>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdatesAllFieldsCorrectly()
        {
            // Arrange
            var curriculumId = 1;
            var existingCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var command = new UpdateCurriculumCommand(curriculumId, 100, 200, 300, 400);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(existingCurriculum);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync((Elm.Domain.Entities.Curriculum c) => c);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .Returns(new CurriculumDto { Id = curriculumId, SubjectId = 100, YearId = 200, DepartmentId = 300, DoctorId = 400 });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Curriculum>(c =>
                c.SubjectId == 100 &&
                c.YearId == 200 &&
                c.DepartmentId == 300 &&
                c.DoctorId == 400)), Times.Once);
        }

        [Theory]
        [InlineData(1, 10, 20, 30, 40)]
        [InlineData(2, 50, 60, 70, 80)]
        [InlineData(3, 100, 200, 300, 400)]
        public async Task Handle_WithVariousInputs_UpdatesCorrectly(int id, int subjectId, int yearId, int departmentId, int doctorId)
        {
            // Arrange
            var existingCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = id,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var command = new UpdateCurriculumCommand(id, subjectId, yearId, departmentId, doctorId);

            var updatedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = id,
                SubjectId = subjectId,
                YearId = yearId,
                DepartmentId = departmentId,
                DoctorId = doctorId
            };

            var expectedDto = new CurriculumDto
            {
                Id = id,
                SubjectId = subjectId,
                YearId = yearId,
                DepartmentId = departmentId,
                DoctorId = doctorId
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingCurriculum);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(updatedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(updatedCurriculum))
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
        public async Task Handle_CallsGetByIdFirst()
        {
            // Arrange
            var curriculumId = 1;
            var existingCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var command = new UpdateCurriculumCommand(curriculumId, 10, 20, 30, 40);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(existingCurriculum);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(existingCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .Returns(new CurriculumDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithUpdatedEntity()
        {
            // Arrange
            var curriculumId = 1;
            var existingCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var command = new UpdateCurriculumCommand(curriculumId, 10, 20, 30, 40);

            var updatedCurriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 10,
                YearId = 20,
                DepartmentId = 30,
                DoctorId = 40
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(existingCurriculum);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Curriculum>()))
                .ReturnsAsync(updatedCurriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(updatedCurriculum))
                .Returns(new CurriculumDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<CurriculumDto>(updatedCurriculum), Times.Once);
        }
    }
}
