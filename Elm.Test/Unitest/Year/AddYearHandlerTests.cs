using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Year.Handlers;
using Moq;

namespace Elm.Test.Unitest.Year
{
    public class AddYearHandlerTests
    {
        private readonly Mock<IYearRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddYearHandler _handler;

        public AddYearHandlerTests()
        {
            _repositoryMock = new Mock<IYearRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddYearHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsSuccessWithYearDto()
        {
            // Arrange
            var command = new AddYearCommand("First Year", 1);

            var addedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "First Year",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "First Year",
                CollegeId = 1
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Year>()))
                .ReturnsAsync(addedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(addedYear))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.CollegeId, result.Data.CollegeId);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Elm.Domain.Entities.Year>(y =>
                y.Name == command.name &&
                y.CollegeId == command.collegeId)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ReturnsFailure()
        {
            // Arrange
            var command = new AddYearCommand("Test Year", 1);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Year>()))
                .ReturnsAsync((Elm.Domain.Entities.Year?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add year", result.Message);
            _mapperMock.Verify(m => m.Map<YearDto>(It.IsAny<Elm.Domain.Entities.Year>()), Times.Never);
        }

        [Theory]
        [InlineData("First Year", 1)]
        [InlineData("Second Year", 2)]
        [InlineData("Third Year", 3)]
        public async Task Handle_WithVariousInputs_CreatesCorrectYear(string name, int collegeId)
        {
            // Arrange
            var command = new AddYearCommand(name, collegeId);

            var addedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = name,
                CollegeId = collegeId
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = name,
                CollegeId = collegeId
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Year>()))
                .ReturnsAsync(addedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(addedYear))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(collegeId, result.Data?.CollegeId);
        }

        [Fact]
        public async Task Handle_CallsRepositoryWithCorrectEntity()
        {
            // Arrange
            var command = new AddYearCommand("Test Year", 5);
            Elm.Domain.Entities.Year? capturedYear = null;

            var addedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 5
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 5
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Year>()))
                .Callback<Elm.Domain.Entities.Year>(y => capturedYear = y)
                .ReturnsAsync(addedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(addedYear))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedYear);
            Assert.Equal(command.name, capturedYear.Name);
            Assert.Equal(command.collegeId, capturedYear.CollegeId);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var command = new AddYearCommand("Fourth Year", 2);

            var addedYear = new Elm.Domain.Entities.Year
            {
                Id = 4,
                Name = "Fourth Year",
                CollegeId = 2
            };

            var expectedDto = new YearDto
            {
                Id = 4,
                Name = "Fourth Year",
                CollegeId = 2
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Year>()))
                .ReturnsAsync(addedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(addedYear))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<YearDto>(addedYear), Times.Once);
        }
    }
}
