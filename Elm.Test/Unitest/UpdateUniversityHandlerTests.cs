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
    public class UpdateUniversityHandlerTests
    {
        private readonly Mock<IGenericRepository<University>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateUniversityHandler _handler;

        public UpdateUniversityHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<University>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateUniversityHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessWithUpdatedUniversityDto()
        {
            // Arrange
            var command = new UpdateUniversityCommand(1, "Updated University");
            var existingUniversity = new University { Id = 1, Name = "Old University" };
            var updatedUniversity = new University { Id = 1, Name = "Updated University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "Updated University" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Updated University", result.Data.Name);
        }

        [Fact]
        public async Task Handle_UniversityNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateUniversityCommand(999, "Updated University");

            _mockRepository
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((University?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("University not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryGetByIdAsync()
        {
            // Arrange
            var command = new UpdateUniversityCommand(5, "Test University");
            var existingUniversity = new University { Id = 5, Name = "Old Name" };
            var updatedUniversity = new University { Id = 5, Name = "Test University" };
            var expectedDto = new UniversityDto { Id = 5, Name = "Test University" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(5))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryUpdateAsync()
        {
            // Arrange
            var command = new UpdateUniversityCommand(1, "New Name");
            var existingUniversity = new University { Id = 1, Name = "Old Name" };
            var updatedUniversity = new University { Id = 1, Name = "New Name" };
            var expectedDto = new UniversityDto { Id = 1, Name = "New Name" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<University>(u => u.Name == "New Name")), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesUniversityName()
        {
            // Arrange
            var command = new UpdateUniversityCommand(1, "Updated Name");
            var existingUniversity = new University { Id = 1, Name = "Original Name" };
            var updatedUniversity = new University { Id = 1, Name = "Updated Name" };
            var expectedDto = new UniversityDto { Id = 1, Name = "Updated Name" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .Callback<University>(u => Assert.Equal("Updated Name", u.Name))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Updated Name", existingUniversity.Name);
        }

        [Fact]
        public async Task Handle_UniversityNotFound_DoesNotCallUpdateAsync()
        {
            // Arrange
            var command = new UpdateUniversityCommand(999, "Test");

            _mockRepository
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((University?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<University>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UniversityNotFound_DoesNotCallMapper()
        {
            // Arrange
            var command = new UpdateUniversityCommand(999, "Test");

            _mockRepository
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((University?)null);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<UniversityDto>(It.IsAny<University>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsStatusCode200()
        {
            // Arrange
            var command = new UpdateUniversityCommand(1, "University");
            var existingUniversity = new University { Id = 1, Name = "Old" };
            var updatedUniversity = new University { Id = 1, Name = "University" };
            var expectedDto = new UniversityDto { Id = 1, Name = "University" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Handle_EmptyName_StillUpdatesUniversity()
        {
            // Arrange
            var command = new UpdateUniversityCommand(1, string.Empty);
            var existingUniversity = new University { Id = 1, Name = "Old Name" };
            var updatedUniversity = new University { Id = 1, Name = string.Empty };
            var expectedDto = new UniversityDto { Id = 1, Name = string.Empty };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingUniversity);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<University>()))
                .ReturnsAsync(updatedUniversity);

            _mockMapper
                .Setup(m => m.Map<UniversityDto>(It.IsAny<University>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(string.Empty, result.Data?.Name);
        }
    }
}
