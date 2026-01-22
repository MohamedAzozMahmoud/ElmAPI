using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.Commands;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.University.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest
{
    public class AddUniversityHandlerTests
    {
        private readonly Mock<IGenericRepository<University>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddUniversityHandler _handler;

        public AddUniversityHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<University>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new AddUniversityHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessWithUniversityDto()
        {
            // Arrange
            var command = new AddUniversityCommand("Test University");
            var expectedUniversity = new University { Id = 1, Name = "Test University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "Test University" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<University>()))
                .ReturnsAsync(expectedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryAddAsync()
        {
            // Arrange
            var command = new AddUniversityCommand("New University");
            var expectedUniversity = new University { Id = 1, Name = "New University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "New University" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<University>()))
                .ReturnsAsync(expectedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.Is<University>(u => u.Name == "New University")), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsMapperWithCorrectUniversity()
        {
            // Arrange
            var command = new AddUniversityCommand("Mapped University");
            var expectedUniversity = new University { Id = 1, Name = "Mapped University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "Mapped University" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<University>()))
                .ReturnsAsync(expectedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<UniversityDto>(It.IsAny<University>()), Times.Once);
        }

        [Fact]
        public async Task Handle_EmptyName_StillCreatesUniversity()
        {
            // Arrange
            var command = new AddUniversityCommand(string.Empty);
            var expectedUniversity = new University { Id = 1, Name = string.Empty };
            var expectedDto = new UniversityDto { Id = 1, Name = string.Empty };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<University>()))
                .ReturnsAsync(expectedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(string.Empty, result.Data?.Name);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsStatusCode200()
        {
            // Arrange
            var command = new AddUniversityCommand("University");
            var expectedUniversity = new University { Id = 1, Name = "University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "University" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<University>()))
                .ReturnsAsync(expectedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }
    }
}
