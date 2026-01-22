using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.University.DTOs;
using Elm.Application.Contracts.Features.University.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.University.Handlers;
using Moq;

namespace Elm.Test.Unitest
{
    public class GetUniversityByIdHandlerTests
    {
        private readonly Mock<IUniversityRepository> _mockRepository;
        private readonly GetUniversityByIdHandler _handler;

        public GetUniversityByIdHandlerTests()
        {
            _mockRepository = new Mock<IUniversityRepository>();
            _handler = new GetUniversityByIdHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_UniversityExists_ReturnsSuccessWithUniversityDetails()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(1);
            var expectedUniversity = new UniversityDetialsDto
            {
                Id = 1,
                Name = "Test University",
                ImageName = "test-image.jpg"
            };

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(1))
                .ReturnsAsync(expectedUniversity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Test University", result.Data.Name);
            Assert.Equal("test-image.jpg", result.Data.ImageName);
        }

        [Fact]
        public async Task Handle_UniversityNotFound_ReturnsFailure()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(999);

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(999))
                .ReturnsAsync((UniversityDetialsDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("University not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ValidQuery_CallsRepositoryUniversityDetialsAsync()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(5);
            var expectedUniversity = new UniversityDetialsDto
            {
                Id = 5,
                Name = "Test University",
                ImageName = "test.jpg"
            };

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(5))
                .ReturnsAsync(expectedUniversity);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UniversityDetialsAsync(5), Times.Once);
        }

        [Fact]
        public async Task Handle_UniversityExists_ReturnsStatusCode200()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(1);
            var expectedUniversity = new UniversityDetialsDto
            {
                Id = 1,
                Name = "Test University",
                ImageName = "test.jpg"
            };

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(1))
                .ReturnsAsync(expectedUniversity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UniversityNotFound_ReturnsStatusCode404()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(999);

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(999))
                .ReturnsAsync((UniversityDetialsDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_UniversityWithNullImageName_ReturnsSuccess()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(1);
            var expectedUniversity = new UniversityDetialsDto
            {
                Id = 1,
                Name = "Test University",
                ImageName = null!
            };

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(1))
                .ReturnsAsync(expectedUniversity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Data?.ImageName);
        }

        [Fact]
        public async Task Handle_DifferentIds_CallsRepositoryWithCorrectId()
        {
            // Arrange
            var queries = new[] { 1, 10, 100, 1000 };

            foreach (var id in queries)
            {
                var query = new GetUniversityByIdQuery(id);
                var expectedUniversity = new UniversityDetialsDto
                {
                    Id = id,
                    Name = $"University {id}",
                    ImageName = $"image-{id}.jpg"
                };

                _mockRepository
                    .Setup(r => r.UniversityDetialsAsync(id))
                    .ReturnsAsync(expectedUniversity);

                // Act
                var result = await _handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.True(result.IsSuccess);
                Assert.Equal(id, result.Data?.Id);
                _mockRepository.Verify(r => r.UniversityDetialsAsync(id), Times.Once);
            }
        }

        [Fact]
        public async Task Handle_UniversityNotFound_DataIsNull()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(999);

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(999))
                .ReturnsAsync((UniversityDetialsDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Handle_ZeroId_CallsRepository()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(0);

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(0))
                .ReturnsAsync((UniversityDetialsDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UniversityDetialsAsync(0), Times.Once);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_NegativeId_CallsRepository()
        {
            // Arrange
            var query = new GetUniversityByIdQuery(-1);

            _mockRepository
                .Setup(r => r.UniversityDetialsAsync(-1))
                .ReturnsAsync((UniversityDetialsDto?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UniversityDetialsAsync(-1), Times.Once);
            Assert.False(result.IsSuccess);
        }
    }
}
