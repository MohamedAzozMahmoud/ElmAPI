using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Subject.DTOs;
using Elm.Application.Contracts.Features.Subject.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Subject.Handlers;
using Moq;

namespace Elm.Test.Unitest.Subject
{
    public class GetAllSubjectHandlerTests
    {
        private readonly Mock<ISubjectRepository> _repositoryMock;
        private readonly GetAllSubjectHandler _handler;

        public GetAllSubjectHandlerTests()
        {
            _repositoryMock = new Mock<ISubjectRepository>();
            _handler = new GetAllSubjectHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenSubjectsExist_ReturnsSuccessWithSubjectList()
        {
            // Arrange
            var departmentId = 1;
            var subjects = new List<GetSubjectDto>
            {
                new GetSubjectDto { Id = 1, Name = "Mathematics", Code = "MATH101" },
                new GetSubjectDto { Id = 2, Name = "Physics", Code = "PHY101" },
                new GetSubjectDto { Id = 3, Name = "Chemistry", Code = "CHEM101" }
            };

            var query = new GetAllSubjectQuery(departmentId);

            _repositoryMock.Setup(r => r.GetAllSubjectByDepartmentIdAsync(departmentId))
                .ReturnsAsync(subjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Mathematics", result.Data[0].Name);
            Assert.Equal("Physics", result.Data[1].Name);
            Assert.Equal("Chemistry", result.Data[2].Name);
            _repositoryMock.Verify(r => r.GetAllSubjectByDepartmentIdAsync(departmentId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoSubjectsExist_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var departmentId = 999;
            var emptySubjects = new List<GetSubjectDto>();

            var query = new GetAllSubjectQuery(departmentId);

            _repositoryMock.Setup(r => r.GetAllSubjectByDepartmentIdAsync(departmentId))
                .ReturnsAsync(emptySubjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            _repositoryMock.Verify(r => r.GetAllSubjectByDepartmentIdAsync(departmentId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task Handle_WithVariousDepartmentIds_CallsRepositoryWithCorrectDepartmentId(int departmentId)
        {
            // Arrange
            var subjects = new List<GetSubjectDto>
            {
                new GetSubjectDto { Id = 1, Name = "Test Subject", Code = "TEST001" }
            };

            var query = new GetAllSubjectQuery(departmentId);

            _repositoryMock.Setup(r => r.GetAllSubjectByDepartmentIdAsync(departmentId))
                .ReturnsAsync(subjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetAllSubjectByDepartmentIdAsync(departmentId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsDataDirectlyFromRepository()
        {
            // Arrange
            var departmentId = 1;
            var subjects = new List<GetSubjectDto>
            {
                new GetSubjectDto { Id = 1, Name = "Subject 1", Code = "SUB001" },
                new GetSubjectDto { Id = 2, Name = "Subject 2", Code = "SUB002" }
            };

            var query = new GetAllSubjectQuery(departmentId);

            _repositoryMock.Setup(r => r.GetAllSubjectByDepartmentIdAsync(departmentId))
                .ReturnsAsync(subjects);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, s => s.Name == "Subject 1" && s.Code == "SUB001");
            Assert.Contains(result.Data, s => s.Name == "Subject 2" && s.Code == "SUB002");
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var departmentId = 1;
            var subjects = new List<GetSubjectDto>();

            var query = new GetAllSubjectQuery(departmentId);

            _repositoryMock.Setup(r => r.GetAllSubjectByDepartmentIdAsync(departmentId))
                .ReturnsAsync(subjects);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetAllSubjectByDepartmentIdAsync(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(r => r.GetAllSubjectByDepartmentIdAsync(departmentId), Times.Once);
        }
    }
}
