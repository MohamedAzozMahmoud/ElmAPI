using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Department.Handlers;
using Elm.Domain.Enums;
using Moq;

namespace Elm.Test.Unitest.Department
{
    public class UpdateDepartmentHandlerTests
    {
        private readonly Mock<IDepartmentRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateDepartmentHandler _handler;

        public UpdateDepartmentHandlerTests()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateDepartmentHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDepartmentExists_ReturnsSuccessWithUpdatedDto()
        {
            // Arrange
            var departmentId = 1;
            var existingDepartment = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Old Name",
                IsPaid = false,
                CollegeId = 1
            };

            var command = new UpdateDepartmentCommand(departmentId, "New Name", true, TypeOfDepartment.General);

            var updatedDepartment = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "New Name",
                IsPaid = true,
                CollegeId = 1
            };

            var expectedDto = new DepartmentDto
            {
                Id = departmentId,
                Name = "New Name",
                IsPaid = true,
                Type = "General",
                CollegeId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(updatedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(updatedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.IsPaid, result.Data.IsPaid);
            _repositoryMock.Verify(r => r.GetByIdAsync(departmentId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDepartmentNotFound_ReturnsFailure()
        {
            // Arrange
            var departmentId = 999;
            var command = new UpdateDepartmentCommand(departmentId, "New Name", true, TypeOfDepartment.General);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync((Elm.Domain.Entities.Department?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Department not found", result.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(departmentId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()), Times.Never);
            _mapperMock.Verify(m => m.Map<DepartmentDto>(It.IsAny<Elm.Domain.Entities.Department>()), Times.Never);
        }

        [Fact]
        public async Task Handle_UpdatesNameCorrectly()
        {
            // Arrange
            var departmentId = 1;
            var existingDepartment = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Original Name",
                IsPaid = true,
                CollegeId = 1
            };

            var command = new UpdateDepartmentCommand(departmentId, "Updated Name", true, TypeOfDepartment.General);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync((Elm.Domain.Entities.Department d) => d);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(It.IsAny<Elm.Domain.Entities.Department>()))
                .Returns(new DepartmentDto { Id = departmentId, Name = "Updated Name", IsPaid = true });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Department>(d =>
                d.Name == "Updated Name")), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdatesIsPaidCorrectly()
        {
            // Arrange
            var departmentId = 1;
            var existingDepartment = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Test Department",
                IsPaid = false,
                CollegeId = 1
            };

            var command = new UpdateDepartmentCommand(departmentId, "Test Department", true, TypeOfDepartment.General);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync((Elm.Domain.Entities.Department d) => d);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(It.IsAny<Elm.Domain.Entities.Department>()))
                .Returns(new DepartmentDto { Id = departmentId, Name = "Test Department", IsPaid = true });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Department>(d =>
                d.IsPaid == true)), Times.Once);
        }

        [Theory]
        [InlineData(1, "Department A", true)]
        [InlineData(2, "Department B", false)]
        [InlineData(3, "Department C", true)]
        public async Task Handle_WithVariousInputs_UpdatesCorrectly(int id, string name, bool isPaid)
        {
            // Arrange
            var existingDepartment = new Elm.Domain.Entities.Department
            {
                Id = id,
                Name = "Old Name",
                IsPaid = !isPaid,
                CollegeId = 1
            };

            var command = new UpdateDepartmentCommand(id, name, isPaid, TypeOfDepartment.General);

            var updatedDepartment = new Elm.Domain.Entities.Department
            {
                Id = id,
                Name = name,
                IsPaid = isPaid,
                CollegeId = 1
            };

            var expectedDto = new DepartmentDto
            {
                Id = id,
                Name = name,
                IsPaid = isPaid,
                Type = "General",
                CollegeId = 1
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingDepartment);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(updatedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(updatedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(isPaid, result.Data?.IsPaid);
        }

        [Fact]
        public async Task Handle_PreservesCollegeIdDuringUpdate()
        {
            // Arrange
            var departmentId = 1;
            var originalCollegeId = 5;
            var existingDepartment = new Elm.Domain.Entities.Department
            {
                Id = departmentId,
                Name = "Original Name",
                IsPaid = false,
                CollegeId = originalCollegeId
            };

            var command = new UpdateDepartmentCommand(departmentId, "New Name", true, TypeOfDepartment.General);

            _repositoryMock.Setup(r => r.GetByIdAsync(departmentId))
                .ReturnsAsync(existingDepartment);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync((Elm.Domain.Entities.Department d) => d);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(It.IsAny<Elm.Domain.Entities.Department>()))
                .Returns(new DepartmentDto { Id = departmentId, Name = "New Name", IsPaid = true, CollegeId = originalCollegeId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Elm.Domain.Entities.Department>(d =>
                d.CollegeId == originalCollegeId)), Times.Once);
        }
    }
}
