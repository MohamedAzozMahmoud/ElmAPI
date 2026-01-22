using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.DTOs;
using Elm.Application.Contracts.Features.Roles.Queries;
using Elm.Application.Features.Roles.Handlers;
using Elm.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Elm.Test.Unitest.Roles
{
    public class GetRolesHandlerTests
    {
        private readonly Mock<RoleManager<Role>> _mockRoleManager;
        private readonly GetRolesHandler _handler;

        public GetRolesHandlerTests()
        {
            var store = new Mock<IRoleStore<Role>>();
            _mockRoleManager = new Mock<RoleManager<Role>>(
                store.Object, null!, null!, null!, null!);
            _handler = new GetRolesHandler(_mockRoleManager.Object);
        }

        [Fact]
        public async Task Handle_WhenRolesExist_ReturnsSuccessWithRoles()
        {
            // Arrange
            var query = new GetRolesQuery();
            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" },
                new Role { Name = "Manager" }
            }.AsQueryable();

            _mockRoleManager
                .Setup(r => r.Roles)
                .Returns(roles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());
        }

        [Fact]
        public async Task Handle_WhenNoRolesExist_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetRolesQuery();
            var emptyRoles = new List<Role>().AsQueryable();

            _mockRoleManager
                .Setup(r => r.Roles)
                .Returns(emptyRoles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_ReturnsRoleDtosWithCorrectNames()
        {
            // Arrange
            var query = new GetRolesQuery();
            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            }.AsQueryable();

            _mockRoleManager
                .Setup(r => r.Roles)
                .Returns(roles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var roleList = result.Data!.ToList();
            Assert.Contains(roleList, r => r.Name == "Admin");
            Assert.Contains(roleList, r => r.Name == "User");
        }

        [Fact]
        public async Task Handle_ReturnsIEnumerableOfRoleDto()
        {
            // Arrange
            var query = new GetRolesQuery();
            var roles = new List<Role>
            {
                new Role { Name = "TestRole" }
            }.AsQueryable();

            _mockRoleManager
                .Setup(r => r.Roles)
                .Returns(roles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<RoleDto>>(result.Data);
        }

        [Fact]
        public async Task Handle_AccessesRolesProperty()
        {
            // Arrange
            var query = new GetRolesQuery();
            var roles = new List<Role>().AsQueryable();

            _mockRoleManager
                .Setup(r => r.Roles)
                .Returns(roles);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRoleManager.Verify(r => r.Roles, Times.Once);
        }
    }
}
