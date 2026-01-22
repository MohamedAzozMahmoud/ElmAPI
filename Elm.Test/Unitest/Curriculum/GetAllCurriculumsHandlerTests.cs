using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Curriculum.DTOs;
using Elm.Application.Contracts.Features.Curriculum.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.QuestionsBank.Handlers;
using Moq;

namespace Elm.Test.Unitest.Curriculum
{
    public class GetAllCurriculumsHandlerTests
    {
        private readonly Mock<ICurriculumRepository> _repositoryMock;
        private readonly GetAllCurriculumsHandler _handler;

        public GetAllCurriculumsHandlerTests()
        {
            _repositoryMock = new Mock<ICurriculumRepository>();
            _handler = new GetAllCurriculumsHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCurriculumsExist_ReturnsSuccessWithCurriculumList()
        {
            // Arrange
            var departmentId = 1;
            var yearId = 1;
            var curriculums = new List<GetCurriculumDto>
            {
                new GetCurriculumDto { Id = 1, SubjectName = "Mathematics" },
                new GetCurriculumDto { Id = 2, SubjectName = "Physics" },
                new GetCurriculumDto { Id = 3, SubjectName = "Chemistry" }
            };

            var query = new GetAllCurriculumQuery(departmentId, yearId);

            _repositoryMock.Setup(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId))
                .ReturnsAsync(curriculums);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal("Mathematics", result.Data[0].SubjectName);
            Assert.Equal("Physics", result.Data[1].SubjectName);
            Assert.Equal("Chemistry", result.Data[2].SubjectName);
            _repositoryMock.Verify(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoCurriculumsExist_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var departmentId = 999;
            var yearId = 999;
            var emptyCurriculums = new List<GetCurriculumDto>();

            var query = new GetAllCurriculumQuery(departmentId, yearId);

            _repositoryMock.Setup(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId))
                .ReturnsAsync(emptyCurriculums);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            _repositoryMock.Verify(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 2)]
        [InlineData(10, 3)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectIds(int departmentId, int yearId)
        {
            // Arrange
            var curriculums = new List<GetCurriculumDto>
            {
                new GetCurriculumDto { Id = 1, SubjectName = "Test Subject" }
            };

            var query = new GetAllCurriculumQuery(departmentId, yearId);

            _repositoryMock.Setup(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId))
                .ReturnsAsync(curriculums);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsDataDirectlyFromRepository()
        {
            // Arrange
            var departmentId = 1;
            var yearId = 1;
            var curriculums = new List<GetCurriculumDto>
            {
                new GetCurriculumDto { Id = 1, SubjectName = "Subject 1" },
                new GetCurriculumDto { Id = 2, SubjectName = "Subject 2" }
            };

            var query = new GetAllCurriculumQuery(departmentId, yearId);

            _repositoryMock.Setup(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId))
                .ReturnsAsync(curriculums);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Contains(result.Data, c => c.SubjectName == "Subject 1");
            Assert.Contains(result.Data, c => c.SubjectName == "Subject 2");
        }

        [Fact]
        public async Task Handle_RepositoryCalledExactlyOnce()
        {
            // Arrange
            var departmentId = 1;
            var yearId = 1;
            var curriculums = new List<GetCurriculumDto>();

            var query = new GetAllCurriculumQuery(departmentId, yearId);

            _repositoryMock.Setup(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(departmentId, yearId))
                .ReturnsAsync(curriculums);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetAllCurriculumsByDeptIdAndYearIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}
