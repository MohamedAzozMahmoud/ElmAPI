using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Department.Handlers;
using Moq;

namespace Elm.Test.Unitest.Department
{
    public class GetAllDepartmentHandlerTests
    {
        private readonly Mock<IDepartmentRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllDepartmentHandler _handler;

        public GetAllDepartmentHandlerTests()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllDepartmentHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDepartmentsExist_ReturnsSuccessWithDepartmentList()
        {
            // Arrange
            var yearId = 1;
            var departments = new List<GetDepartmentDto>
            {
                new GetDepartmentDto { Id = 1, Name = "Computer Science", IsPaid = true, Type = "General" },
                new GetDepartmentDto { Id = 2, Name = "Mathematics", IsPaid = false, Type = "General" }
            };

            var query = new GetAllDepartmentQuery(yearId);

            _repositoryMock.Setup(r => r.GetAllDepartmentInCollegeAsync(yearId))
                .ReturnsAsync(departments);

            _mapperMock.Setup(m => m.Map<List<GetDepartmentDto>>(departments))
                .Returns(departments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Computer Science", result.Data[0].Name);
            Assert.Equal("Mathematics", result.Data[1].Name);
            _repositoryMock.Verify(r => r.GetAllDepartmentInCollegeAsync(yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoDepartmentsExist_ReturnsSuccessWithEmptyList()
        {
            // Arrange
            var yearId = 999;
            var emptyDepartments = new List<GetDepartmentDto>();

            var query = new GetAllDepartmentQuery(yearId);

            _repositoryMock.Setup(r => r.GetAllDepartmentInCollegeAsync(yearId))
                .ReturnsAsync(emptyDepartments);

            _mapperMock.Setup(m => m.Map<List<GetDepartmentDto>>(emptyDepartments))
                .Returns(emptyDepartments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            _repositoryMock.Verify(r => r.GetAllDepartmentInCollegeAsync(yearId), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task Handle_WithVariousYearIds_CallsRepositoryWithCorrectYearId(int yearId)
        {
            // Arrange
            var departments = new List<GetDepartmentDto>
            {
                new GetDepartmentDto { Id = 1, Name = "Test Department", IsPaid = true, Type = "General" }
            };

            var query = new GetAllDepartmentQuery(yearId);

            _repositoryMock.Setup(r => r.GetAllDepartmentInCollegeAsync(yearId))
                .ReturnsAsync(departments);

            _mapperMock.Setup(m => m.Map<List<GetDepartmentDto>>(departments))
                .Returns(departments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.GetAllDepartmentInCollegeAsync(yearId), Times.Once);
        }

        [Fact]
        public async Task Handle_MapsCorrectlyFromRepository_ReturnsProperlyMappedData()
        {
            // Arrange
            var yearId = 1;
            var departments = new List<GetDepartmentDto>
            {
                new GetDepartmentDto { Id = 1, Name = "Engineering", IsPaid = true, Type = "Technical" },
                new GetDepartmentDto { Id = 2, Name = "Arts", IsPaid = false, Type = "General" },
                new GetDepartmentDto { Id = 3, Name = "Medicine", IsPaid = true, Type = "Medical" }
            };

            var query = new GetAllDepartmentQuery(yearId);

            _repositoryMock.Setup(r => r.GetAllDepartmentInCollegeAsync(yearId))
                .ReturnsAsync(departments);

            _mapperMock.Setup(m => m.Map<List<GetDepartmentDto>>(departments))
                .Returns(departments);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Contains(result.Data, d => d.Name == "Engineering" && d.IsPaid);
            Assert.Contains(result.Data, d => d.Name == "Arts" && !d.IsPaid);
            Assert.Contains(result.Data, d => d.Name == "Medicine" && d.IsPaid);
        }
    }
}
