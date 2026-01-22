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
    public class AddDepartmentHandlerTests
    {
        private readonly Mock<IDepartmentRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddDepartmentHandler _handler;

        public AddDepartmentHandlerTests()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddDepartmentHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsSuccessWithDepartmentDto()
        {
            // Arrange
            var command = new AddDepartmentCommand("Computer Science", true, TypeOfDepartment.General, 1);

            var addedDepartment = new Elm.Domain.Entities.Department
            {
                Id = 1,
                Name = "Computer Science",
                IsPaid = true,
                CollegeId = 1
            };

            var expectedDto = new DepartmentDto
            {
                Id = 1,
                Name = "Computer Science",
                IsPaid = true,
                Type = "General",
                CollegeId = 1
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(addedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(addedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedDto.Id, result.Data.Id);
            Assert.Equal(expectedDto.Name, result.Data.Name);
            Assert.Equal(expectedDto.IsPaid, result.Data.IsPaid);
            Assert.Equal(expectedDto.CollegeId, result.Data.CollegeId);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Elm.Domain.Entities.Department>(d =>
                d.Name == command.Name &&
                d.IsPaid == command.IsPaid &&
                d.CollegeId == command.collegeId)), Times.Once);
        }

        [Fact]
        public async Task Handle_WithPaidDepartment_SetsIsPaidCorrectly()
        {
            // Arrange
            var command = new AddDepartmentCommand("Premium Department", true, TypeOfDepartment.General, 2);

            var addedDepartment = new Elm.Domain.Entities.Department
            {
                Id = 2,
                Name = "Premium Department",
                IsPaid = true,
                CollegeId = 2
            };

            var expectedDto = new DepartmentDto
            {
                Id = 2,
                Name = "Premium Department",
                IsPaid = true,
                Type = "General",
                CollegeId = 2
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(addedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(addedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data?.IsPaid);
        }

        [Fact]
        public async Task Handle_WithUnpaidDepartment_SetsIsPaidCorrectly()
        {
            // Arrange
            var command = new AddDepartmentCommand("Free Department", false, TypeOfDepartment.General, 3);

            var addedDepartment = new Elm.Domain.Entities.Department
            {
                Id = 3,
                Name = "Free Department",
                IsPaid = false,
                CollegeId = 3
            };

            var expectedDto = new DepartmentDto
            {
                Id = 3,
                Name = "Free Department",
                IsPaid = false,
                Type = "General",
                CollegeId = 3
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(addedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(addedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data?.IsPaid);
        }

        [Theory]
        [InlineData("Engineering", true, 1)]
        [InlineData("Medicine", false, 2)]
        [InlineData("Law", true, 3)]
        public async Task Handle_WithVariousInputs_CreatesCorrectDepartment(string name, bool isPaid, int collegeId)
        {
            // Arrange
            var command = new AddDepartmentCommand(name, isPaid, TypeOfDepartment.General, collegeId);

            var addedDepartment = new Elm.Domain.Entities.Department
            {
                Id = 1,
                Name = name,
                IsPaid = isPaid,
                CollegeId = collegeId
            };

            var expectedDto = new DepartmentDto
            {
                Id = 1,
                Name = name,
                IsPaid = isPaid,
                Type = "General",
                CollegeId = collegeId
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .ReturnsAsync(addedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(addedDepartment))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(name, result.Data?.Name);
            Assert.Equal(isPaid, result.Data?.IsPaid);
            Assert.Equal(collegeId, result.Data?.CollegeId);
        }

        [Fact]
        public async Task Handle_CallsMapperWithCorrectEntity()
        {
            // Arrange
            var command = new AddDepartmentCommand("Test Department", true, TypeOfDepartment.General, 1);
            Elm.Domain.Entities.Department? capturedDepartment = null;

            var addedDepartment = new Elm.Domain.Entities.Department
            {
                Id = 1,
                Name = "Test Department",
                IsPaid = true,
                CollegeId = 1
            };

            var expectedDto = new DepartmentDto
            {
                Id = 1,
                Name = "Test Department",
                IsPaid = true,
                Type = "General",
                CollegeId = 1
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Elm.Domain.Entities.Department>()))
                .Callback<Elm.Domain.Entities.Department>(d => capturedDepartment = d)
                .ReturnsAsync(addedDepartment);

            _mapperMock.Setup(m => m.Map<DepartmentDto>(addedDepartment))
                .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedDepartment);
            Assert.Equal(command.Name, capturedDepartment.Name);
            Assert.Equal(command.IsPaid, capturedDepartment.IsPaid);
            Assert.Equal(command.collegeId, capturedDepartment.CollegeId);
            _mapperMock.Verify(m => m.Map<DepartmentDto>(addedDepartment), Times.Once);
        }
    }
}
