using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.College.DTOs;
using Elm.Application.Contracts.Features.College.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.College.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest.College
{
    public class GetCollegeByIdHandlerTests
    {
        private readonly Mock<ICollegeRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetCollegeByIdHandler _handler;

        public GetCollegeByIdHandlerTests()
        {
            _mockRepository = new Mock<ICollegeRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetCollegeByIdHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenCollegeExists_ReturnsSuccessResult()
        {
            // Arrange
            var collegeId = 1;
            var query = new GetCollegeByIdQuery(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Test College",
                UniversityId = 1,
                ImgId = 1
            };
            var collegeDto = new GetCollegeDto
            {
                Id = collegeId,
                Name = "Test College",
                ImagName = "test.jpg"
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockMapper
                .Setup(m => m.Map<GetCollegeDto>(college))
                .Returns(collegeDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(collegeId, result.Data.Id);
            Assert.Equal("Test College", result.Data.Name);
            _mockRepository.Verify(r => r.GetByIdAsync(collegeId), Times.Once);
            _mockMapper.Verify(m => m.Map<GetCollegeDto>(college), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCollegeNotFound_ReturnsFailureResult()
        {
            // Arrange
            var collegeId = 999;
            var query = new GetCollegeByIdQuery(collegeId);

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync((Domain.Entities.College)null!);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("College not found.", result.Message);
            _mockRepository.Verify(r => r.GetByIdAsync(collegeId), Times.Once);
            _mockMapper.Verify(m => m.Map<GetCollegeDto>(It.IsAny<Domain.Entities.College>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ReturnsFailureResultWithErrorMessage()
        {
            // Arrange
            var collegeId = 1;
            var query = new GetCollegeByIdQuery(collegeId);
            var exceptionMessage = "Database connection failed";

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(exceptionMessage, result.Message);
            _mockRepository.Verify(r => r.GetByIdAsync(collegeId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidId_CallsRepositoryWithCorrectId()
        {
            // Arrange
            var collegeId = 42;
            var query = new GetCollegeByIdQuery(collegeId);
            var college = new Domain.Entities.College
            {
                Id = collegeId,
                Name = "Engineering College",
                UniversityId = 5
            };
            var collegeDto = new GetCollegeDto
            {
                Id = collegeId,
                Name = "Engineering College"
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(collegeId))
                .ReturnsAsync(college);

            _mockMapper
                .Setup(m => m.Map<GetCollegeDto>(college))
                .Returns(collegeDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(42), Times.Once);
        }
    }
}
