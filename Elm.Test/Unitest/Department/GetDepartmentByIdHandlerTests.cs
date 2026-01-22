using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Department.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest.Department
{
    public class GetDepartmentByIdHandlerTests
    {
        private readonly Mock<IDepartmentRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetDepartmentByIdHandler _handler;

        public GetDepartmentByIdHandlerTests()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetDepartmentByIdHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDepartmentExists_ReturnsSuccessWithDepartmentDto()
        {
            // Arrange
            var departmentId = 1;
            var department = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Computer Science",
                IsPaid = true,
                CollegeId = 1
            };

            var expectedDto = new GetDepartmentDto
            {
                Id = departmentId,
                Name = "Computer Science",
                IsPaid = true,
                Type = "General"
            };

            var query = new GetDepartmentByIdQuery(departmentId);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(department);

            _mapperMock.Setup(m => m.Map<GetDepartmentDto>(department))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.IsPaid, result.Data.IsPaid);
            _repositoryMock.Verify(r => r.GetByIdAsync(departmentId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetDepartmentDto>(department), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDepartmentNotFound_ReturnsFailure()
        {
            // Arrange
            var departmentId = 999;
            var query = new GetDepartmentByIdQuery(departmentId);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync((Elm.Domain.Entities.Department?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Department not found.", result.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(departmentId), Times.Once);
            _mapperMock.Verify(m => m.Map<GetDepartmentDto>(It.IsAny<Elm.Domain.Entities.Department>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(500)]
        public async Task Handle_WithVariousIds_CallsRepositoryWithCorrectId(int departmentId)
        {
            // Arrange
            var department = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Test Department",
                IsPaid = false,
                CollegeId = 1
            };

            var expectedDto = new GetDepartmentDto
            {
                Id = departmentId,
                Name = "Test Department",
                IsPaid = false,
                Type = "General"
            };

            var query = new GetDepartmentByIdQuery(departmentId);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(department);

            _mapperMock.Setup(m => m.Map<GetDepartmentDto>(department))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(departmentId, result.Data?.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(departmentId), Times.Once);
        }
    }
}
