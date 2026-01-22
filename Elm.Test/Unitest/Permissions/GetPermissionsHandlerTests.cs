using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Permissions.DTOs;
using Elm.Application.Contracts.Features.Permissions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Permissions.Handlers;
using Moq;
using PermissionsEntity = Elm.Domain.Entities.Permissions;

namespace Elm.Test.Unitest.Permissions
{
    public class GetPermissionsHandlerTests
    {
        private readonly Mock<IGenericRepository<PermissionsEntity>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetPermissionsHandler _handler;

        public GetPermissionsHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<PermissionsEntity>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetPermissionsHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenPermissionsExist_ReturnsSuccessWithPermissions()
        {
            // Arrange
            var query = new GetAllPermissionsQuery();
            var permissions = new List<PermissionsEntity>
            {
                new PermissionsEntity { Id = 1, Name = "Read" },
                new PermissionsEntity { Id = 2, Name = "Write" },
                new PermissionsEntity { Id = 3, Name = "Delete" }
            };
            var permissionDtos = new List<PermissionDto>
            {
                new PermissionDto { Id = 1, Name = "Read" },
                new PermissionDto { Id = 2, Name = "Write" },
                new PermissionDto { Id = 3, Name = "Delete" }
            };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(permissions);

            _mockMapper
                .Setup(m => m.Map<List<PermissionDto>>(permissions))
                .Returns(permissionDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<List<PermissionDto>>(permissions), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoPermissionsExist_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetAllPermissionsQuery();
            var emptyPermissions = new List<PermissionsEntity>();
            var emptyDtos = new List<PermissionDto>();

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyPermissions);

            _mockMapper
                .Setup(m => m.Map<List<PermissionDto>>(emptyPermissions))
                .Returns(emptyDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WhenCalled_CallsGetAllAsync()
        {
            // Arrange
            var query = new GetAllPermissionsQuery();

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<PermissionsEntity>());

            _mockMapper
                .Setup(m => m.Map<List<PermissionDto>>(It.IsAny<List<PermissionsEntity>>()))
                .Returns(new List<PermissionDto>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToPermissionDtos()
        {
            // Arrange
            var query = new GetAllPermissionsQuery();
            var permissions = new List<PermissionsEntity>
            {
                new PermissionsEntity { Id = 1, Name = "Admin" }
            };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(permissions);

            _mockMapper
                .Setup(m => m.Map<List<PermissionDto>>(permissions))
                .Returns(new List<PermissionDto> { new PermissionDto { Id = 1, Name = "Admin" } });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<List<PermissionDto>>(permissions), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsIEnumerableOfPermissionDto()
        {
            // Arrange
            var query = new GetAllPermissionsQuery();
            var permissions = new List<PermissionsEntity>
            {
                new PermissionsEntity { Id = 1, Name = "Permission1" },
                new PermissionsEntity { Id = 2, Name = "Permission2" }
            };
            var permissionDtos = new List<PermissionDto>
            {
                new PermissionDto { Id = 1, Name = "Permission1" },
                new PermissionDto { Id = 2, Name = "Permission2" }
            };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(permissions);

            _mockMapper
                .Setup(m => m.Map<List<PermissionDto>>(permissions))
                .Returns(permissionDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<PermissionDto>>(result.Data);
        }
    }
}
