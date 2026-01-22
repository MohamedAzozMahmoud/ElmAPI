using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Year.Commands;
using Elm.Application.Contracts.Features.Year.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Year.Handlers;
using Moq;

namespace Elm.Test.Unitest.Year
{
    public class UpdateYearHandlerTests
    {
        private readonly Mock<IYearRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateYearHandler _handler;

        public UpdateYearHandlerTests()
        {
            _repositoryMock = new Mock<IYearRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateYearHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUpdateSucceeds_ReturnsSuccessWithUpdatedDto()
        {
            // Arrange
            var command = new UpdateYearCommand(1, "Updated Year Name");

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Updated Year Name",
                CollegeId = 1
            };

            var updatedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Updated Year Name",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "Updated Year Name",
                CollegeId = 1
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync(updatedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(updatedYear))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            _repositoryMock.Verify(r => r.UpdateAsync(mappedYear), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUpdateFails_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateYearCommand(999, "Non-existent Year");

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = 999,
                Name = "Non-existent Year"
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync((Elm.Domain.Entities.Year?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to update year", result.Message);
            _mapperMock.Verify(m => m.Map<YearDto>(It.IsAny<Elm.Domain.Entities.Year>()), Times.Never);
        }

        [Theory]
        [InlineData(1, "First Year")]
        [InlineData(2, "Second Year")]
        [InlineData(3, "Third Year")]
        public async Task Handle_WithVariousInputs_UpdatesCorrectly(int id, string name)
        {
            // Arrange
            var command = new UpdateYearCommand(id, name);

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = id,
                Name = name
            };

            var updatedYear = new Elm.Domain.Entities.Year
            {
                Id = id,
                Name = name,
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = id,
                Name = name,
                CollegeId = 1
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync(updatedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(updatedYear))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(id, result.Data?.Id);
        }

        [Fact]
        public async Task Handle_MapsCommandToYearEntity()
        {
            // Arrange
            var command = new UpdateYearCommand(1, "Mapped Year");

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Mapped Year"
            };

            var updatedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Mapped Year",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "Mapped Year",
                CollegeId = 1
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync(updatedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(updatedYear))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<Elm.Domain.Entities.Year>(command), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithUpdatedEntity()
        {
            // Arrange
            var command = new UpdateYearCommand(1, "Test Year");

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Test Year"
            };

            var updatedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 2
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 2
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync(updatedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(updatedYear))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<YearDto>(updatedYear), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var command = new UpdateYearCommand(1, "Test Year");

            var mappedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Test Year"
            };

            var updatedYear = new Elm.Domain.Entities.Year
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 1
            };

            var expectedDto = new YearDto
            {
                Id = 1,
                Name = "Test Year",
                CollegeId = 1
            };

            _mapperMock.Setup(m => m.Map<Elm.Domain.Entities.Year>(command))
                .Returns(mappedYear);

            _repositoryMock.Setup(r => r.UpdateAsync(mappedYear))
                .ReturnsAsync(updatedYear);

            _mapperMock.Setup(m => m.Map<YearDto>(updatedYear))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Year>()), Times.Once);
        }
    }
}
