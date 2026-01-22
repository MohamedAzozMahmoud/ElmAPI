using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.Commands;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.College.Handlers;
using Moq;

namespace Elm.Test.Unitest.College
{
    public class AddCollegeHandlerTests
    {
        private readonly Mock<ICollegeRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddCollegeHandler _handler;

        public AddCollegeHandlerTests()
        {
            _mockRepository = new Mock<ICollegeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new AddCollegeHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenCollegeAddedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddCollegeCommand("Test College", 1);
            var addedCollege = new Domain.Entities.College
            {
                Id = 1,
                Name = "Test College",
                UniversityId = 1
            };
            var collegeDto = new CollegeDto
            {
                Id = 1,
                Name = "Test College"
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(addedCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(addedCollege))
                .Returns(collegeDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Test College", result.Data.Name);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.College>()), Times.Once);
            _mockMapper.Verify(m => m.Map<CollegeDto>(addedCollege), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddCollegeCommand("Test College", 1);

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync((Domain.Entities.College)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add college", result.Message);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.College>()), Times.Once);
            _mockMapper.Verify(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenCalled_CreatesCollegeWithCorrectProperties()
        {
            // Arrange
            var collegeName = "Engineering College";
            var universityId = 5;
            var command = new AddCollegeCommand(collegeName, universityId);
            Domain.Entities.College capturedCollege = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.College>()))
                .Callback<Domain.Entities.College>(c => capturedCollege = c)
                .ReturnsAsync((Domain.Entities.College c) => c);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = 1, Name = collegeName });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedCollege);
            Assert.Equal(collegeName, capturedCollege.Name);
            Assert.Equal(universityId, capturedCollege.UniversityId);
        }

        [Fact]
        public async Task Handle_WithValidCommand_CallsRepositoryAddAsync()
        {
            // Arrange
            var command = new AddCollegeCommand("Science College", 2);
            var addedCollege = new Domain.Entities.College
            {
                Id = 1,
                Name = "Science College",
                UniversityId = 2
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(addedCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(It.IsAny<Domain.Entities.College>()))
                .Returns(new CollegeDto { Id = 1, Name = "Science College" });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.Is<Domain.Entities.College>(
                c => c.Name == "Science College" && c.UniversityId == 2)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCollegeAddedSuccessfully_MapsToCollegeDto()
        {
            // Arrange
            var command = new AddCollegeCommand("Arts College", 3);
            var addedCollege = new Domain.Entities.College
            {
                Id = 10,
                Name = "Arts College",
                UniversityId = 3
            };
            var expectedDto = new CollegeDto
            {
                Id = 10,
                Name = "Arts College"
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.College>()))
                .ReturnsAsync(addedCollege);

            _mockMapper
                .Setup(m => m.Map<CollegeDto>(addedCollege))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(10, result.Data!.Id);
            Assert.Equal("Arts College", result.Data.Name);
        }
    }
}
