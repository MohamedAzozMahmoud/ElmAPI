using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Year.Handlers;
using Moq;

namespace Elm.Test.Unitest.Year
{
    public class GetYearByIdHandlerTests
    {
        private readonly Mock<IYearRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetYearByIdHandler _handler;

        public GetYearByIdHandlerTests()
        {
            _repositoryMock = new Mock<IYearRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetYearByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenYearExists_ReturnsSuccessWithYearDto()
        {
            // Arrange
            var yearId = 1;
            var year = new Elm.Domain.Entities.Year
            {
                Id = yearId,
                Name = "First Year",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = yearId,
                Name = "First Year",
                CollegeId = 1
            };

            var query = new GetYearByIdQuery(yearId);

            _repositoryMock.Setup(r => r.GetByIdAsync(yearId))
                .ReturnsAsync(year);

            _mapperMock.Setup(m => m.Map<YearDto>(year))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.CollegeId, result.Data.CollegeId);
            _repositoryMock.Verify(r => r.GetByIdAsync(yearId), Times.Once);
            _mapperMock.Verify(m => m.Map<YearDto>(year), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenYearNotFound_ReturnsSuccessWithNullData()
        {
            // Arrange
            var yearId = 999;
            var query = new GetYearByIdQuery(yearId);

            _repositoryMock.Setup(r => r.GetByIdAsync(yearId))
                .ReturnsAsync((Elm.Domain.Entities.Year?)null);

            _mapperMock.Setup(m => m.Map<YearDto>(It.IsAny<Elm.Domain.Entities.Year?>()))
                .Returns((YearDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetByIdAsync(yearId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int yearId)
        {
            // Arrange
            var year = new Elm.Domain.Entities.Year
            {
                Id = yearId,
                Name = "Test Year",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = yearId,
                Name = "Test Year",
                CollegeId = 1
            };

            var query = new GetYearByIdQuery(yearId);

            _repositoryMock.Setup(r => r.GetByIdAsync(yearId))
                .ReturnsAsync(year);

            _mapperMock.Setup(m => m.Map<YearDto>(year))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(yearId, result.Data?.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var yearId = 1;
            var year = new Elm.Domain.Entities.Year
            {
                Id = yearId,
                Name = "Second Year",
                CollegeId = 2
            };

            var expectedDto = new YearDto
            {
                Id = yearId,
                Name = "Second Year",
                CollegeId = 2
            };

            var query = new GetYearByIdQuery(yearId);

            _repositoryMock.Setup(r => r.GetByIdAsync(yearId))
                .ReturnsAsync(year);

            _mapperMock.Setup(m => m.Map<YearDto>(year))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<YearDto>(It.Is<Elm.Domain.Entities.Year>(y =>
                y.Id == yearId &&
                y.Name == "Second Year" &&
                y.CollegeId == 2)), Times.Once);
        }
    }
}
