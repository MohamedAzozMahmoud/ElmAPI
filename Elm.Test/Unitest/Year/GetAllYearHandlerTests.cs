using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Features.Year.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Year.Handlers;
using Moq;

namespace Elm.Test.Unitest.Year
{
    public class GetAllYearHandlerTests
    {
        private readonly Mock<IYearRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllYearHandler _handler;

        public GetAllYearHandlerTests()
        {
            _repositoryMock = new Mock<IYearRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllYearHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenYearsExist_ReturnsSuccessWithYearList()
        {
            // Arrange
            var collegeId = 1;
            var years = new List<GetYearDto>
            {
                new GetYearDto { Id = 1, Name = "First Year" },
                new GetYearDto { Id = 2, Name = "Second Year" },
                new GetYearDto { Id = 3, Name = "Third Year" }
            };

            var query = new GetAllYearQuery(collegeId);

            _repositoryMock.Setup(r => r.GetAllYearInCollegeAsync(collegeId))
                .ReturnsAsync(years);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("First Year", result.Data[0].Name);
            Assert.Equal("Second Year", result.Data[1].Name);
            Assert.Equal("Third Year", result.Data[2].Name);
            _repositoryMock.Verify(r => r.GetAllYearInCollegeAsync(collegeId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoYearsExist_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var collegeId = 999;
            var emptyYears = new List<GetYearDto>();

            var query = new GetAllYearQuery(collegeId);

            _repositoryMock.Setup(r => r.GetAllYearInCollegeAsync(collegeId))
                .ReturnsAsync(emptyYears);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            _repositoryMock.Verify(r => r.GetAllYearInCollegeAsync(collegeId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task Handle_WithVariousCollegeIds_CallsRepositoryWithCorrectCollegeId(int collegeId)
        {
            // Arrange
            var years = new List<GetYearDto>
            {
                new GetYearDto { Id = 1, Name = "Test Year" }
            };

            var query = new GetAllYearQuery(collegeId);

            _repositoryMock.Setup(r => r.GetAllYearInCollegeAsync(collegeId))
                .ReturnsAsync(years);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetAllYearInCollegeAsync(collegeId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsDataDirectlyFromRepository()
        {
            // Arrange
            var collegeId = 1;
            var years = new List<GetYearDto>
            {
                new GetYearDto { Id = 1, Name = "Year 1" },
                new GetYearDto { Id = 2, Name = "Year 2" }
            };

            var query = new GetAllYearQuery(collegeId);

            _repositoryMock.Setup(r => r.GetAllYearInCollegeAsync(collegeId))
                .ReturnsAsync(years);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, y => y.Name == "Year 1");
            Assert.Contains(result.Data, y => y.Name == "Year 2");
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var collegeId = 1;
            var years = new List<GetYearDto>();

            var query = new GetAllYearQuery(collegeId);

            _repositoryMock.Setup(r => r.GetAllYearInCollegeAsync(collegeId))
                .ReturnsAsync(years);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetAllYearInCollegeAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetAllYearInCollegeAsync(collegeId), Times.Once);
        }
    }
}
