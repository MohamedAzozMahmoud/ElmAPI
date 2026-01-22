using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Subject.Handlers;
using Moq;

namespace Elm.Test.Unitest.Subject
{
    public class GetSubjectByIdHandlerTests
    {
        private readonly Mock<ISubjectRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSubjectByIdHandler _handler;

        public GetSubjectByIdHandlerTests()
        {
            _repositoryMock = new Mock<ISubjectRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSubjectByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenSubjectExists_ReturnsSuccessWithSubjectDto()
        {
            // Arrange
            var subjectId = 1;
            var subject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Mathematics",
                Code = "MATH101"
            };

            var expectedDto = new GetSubjectDto
            {
                Id = subjectId,
                Name = "Mathematics",
                Code = "MATH101"
            };

            var query = new GetSubjectByIdQuery(subjectId);

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(subject);

            _mapperMock.Setup(m => m.Map<GetSubjectDto>(subject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.Code, result.Data.Code);
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetSubjectDto>(subject), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSubjectNotFound_ReturnsFailureWith404()
        {
            // Arrange
            var subjectId = 999;
            var query = new GetSubjectByIdQuery(subjectId);

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync((Elm.Domain.Entities.Subject?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Subject not found.", result.Message);
            Assert.Equal(404, result.StatusCode);
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetSubjectDto>(It.IsAny<Elm.Domain.Entities.Subject>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int subjectId)
        {
            // Arrange
            var subject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Test Subject",
                Code = "TEST001"
            };

            var expectedDto = new GetSubjectDto
            {
                Id = subjectId,
                Name = "Test Subject",
                Code = "TEST001"
            };

            var query = new GetSubjectByIdQuery(subjectId);

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(subject);

            _mapperMock.Setup(m => m.Map<GetSubjectDto>(subject))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subjectId, result.Data?.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(subjectId), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var subjectId = 1;
            var subject = new Elm.Domain.Entities.Subject
            {
                Id = subjectId,
                Name = "Physics",
                Code = "PHY101"
            };

            var expectedDto = new GetSubjectDto
            {
                Id = subjectId,
                Name = "Physics",
                Code = "PHY101"
            };

            var query = new GetSubjectByIdQuery(subjectId);

            _repositoryMock.Setup(r => r.GetByIdAsync(subjectId))
                .ReturnsAsync(subject);

            _mapperMock.Setup(m => m.Map<GetSubjectDto>(subject))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<GetSubjectDto>(It.Is<Elm.Domain.Entities.Subject>(s =>
                s.Id == subjectId &&
                s.Name == "Physics" &&
                s.Code == "PHY101")), Times.Once);
        }
    }
}
