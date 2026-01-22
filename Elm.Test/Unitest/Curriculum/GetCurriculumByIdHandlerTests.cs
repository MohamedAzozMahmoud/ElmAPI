using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.Curriculum
{
    public class GetCurriculumByIdHandlerTests
    {
        private readonly Mock<ICurriculumRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCurriculumByIdHandler _handler;

        public GetCurriculumByIdHandlerTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetCurriculumByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCurriculumExists_ReturnsSuccessWithCurriculumDto()
        {
            // Arrange
            var curriculumId = 1;
            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var expectedDto = new CurriculumDto
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var query = new GetCurriculumByIdQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(curriculum))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.SubjectId, result.Data.SubjectId);
            Assert.Equal(expectedDto.YearId, result.Data.YearId);
            Assert.Equal(expectedDto.DepartmentId, result.Data.DepartmentId);
            Assert.Equal(expectedDto.DoctorId, result.Data.DoctorId);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _mapperMock.Verify(m => m.Map<CurriculumDto>(curriculum), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurriculumNotFound_ReturnsFailure()
        {
            // Arrange
            var curriculumId = 999;
            var query = new GetCurriculumByIdQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync((Elm.Domain.Entities.Curriculum?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Curriculum not found", result.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
            _mapperMock.Verify(m => m.Map<CurriculumDto>(It.IsAny<Elm.Domain.Entities.Curriculum>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int curriculumId)
        {
            // Arrange
            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var expectedDto = new CurriculumDto
            {
                Id = curriculumId,
                SubjectId = 1,
                YearId = 1,
                DepartmentId = 1,
                DoctorId = 1
            };

            var query = new GetCurriculumByIdQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(curriculum))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(curriculumId, result.Data?.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(curriculumId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var curriculumId = 1;
            var curriculum = new Elm.Domain.Entities.Curriculum
            {
                Id = curriculumId,
                SubjectId = 2,
                YearId = 3,
                DepartmentId = 4,
                DoctorId = 5
            };

            var expectedDto = new CurriculumDto
            {
                Id = curriculumId,
                SubjectId = 2,
                YearId = 3,
                DepartmentId = 4,
                DoctorId = 5
            };

            var query = new GetCurriculumByIdQuery(curriculumId);

            _repositoryMock.Setup(r => r.GetByIdAsync(curriculumId))
                .ReturnsAsync(curriculum);

            _mapperMock.Setup(m => m.Map<CurriculumDto>(curriculum))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<CurriculumDto>(It.Is<Elm.Domain.Entities.Curriculum>(c =>
                c.Id == curriculumId &&
                c.SubjectId == 2 &&
                c.YearId == 3 &&
                c.DepartmentId == 4 &&
                c.DoctorId == 5)), Times.Once);
        }
    }
}
