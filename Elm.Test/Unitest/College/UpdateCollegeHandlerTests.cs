using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.College.Handlers;
using Moq;

namespace Elm.Test.Unitest.College
{
    public class UpdateCollegeHandlerTests
    {
        private readonly Mock<ICollegeRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateCollegeHandler _handler;

        public UpdateCollegeHandlerTests()
        {
            _mockRepository = new Mock<ICollegeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateCollegeHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenCollegeExistsAndUpdateSucceeds_ReturnsSuccessResult()
        {
            // Arrange
            var collegeId = 1;
            var newName = "Updated College Name";
            var command = new UpdateCollegeCommand(collegeId, newName);
            var existingCollege = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Original College Name",
                UniversityId = 1
            };
            var updatedCollegeDto = new CollegeDto
            {
                Id = collegeId,
                Name = newName
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(existingCollege);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(existingCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(updatedCollegeDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(collegeId, result.Data.Id);
            Assert.Equal(newName, result.Data.Name);
        }

        [Fact]
        public async Task Handle_WhenCollegeNotFound_ReturnsFailureResult()
        {
            // Arrange
            var collegeId = 999;
            var command = new UpdateCollegeCommand(collegeId, "New Name");

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync((Domain.Entities.College)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("College not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUpdating_SetsCollegeNameCorrectly()
        {
            // Arrange
            var collegeId = 1;
            var newName = "New College Name";
            var command = new UpdateCollegeCommand(collegeId, newName);
            var existingCollege = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Old Name",
                UniversityId = 1
            };
            Domain.Entities.College updatedCollege = null!;

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(existingCollege);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()))
                .Callback<Domain.Entities.College>(c => updatedCollege = c)
                .ReturnsAsync(existingCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = collegeId, Name = newName });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newName, existingCollege.Name);
        }

        [Fact]
        public async Task Handle_CallsRepositoryGetByIdAsync_WithCorrectId()
        {
            // Arrange
            var collegeId = 42;
            var command = new UpdateCollegeCommand(collegeId, "Test Name");
            var existingCollege = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test",
                UniversityId = 1
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(existingCollege);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(existingCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = collegeId, Name = "Test Name" });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUpdateSucceeds_CallsUpdateAsyncOnRepository()
        {
            // Arrange
            var collegeId = 1;
            var command = new UpdateCollegeCommand(collegeId, "Updated Name");
            var existingCollege = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Original Name",
                UniversityId = 1
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(existingCollege);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(existingCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = collegeId, Name = "Updated Name" });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(existingCollege), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenUpdateSucceeds_MapsCollegeToDto()
        {
            // Arrange
            var collegeId = 1;
            var newName = "Mapped College";
            var command = new UpdateCollegeCommand(collegeId, newName);
            var existingCollege = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Original",
                UniversityId = 1
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(existingCollege);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(existingCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = collegeId, Name = newName });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()), Times.Once);
            Assert.Equal(newName, result.Data!.Name);
        }
    }
}
