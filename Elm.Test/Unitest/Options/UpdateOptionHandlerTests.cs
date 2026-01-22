using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.Commands;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Options.Handlers;
using Elm.Domain.Entities;
using Moq;

namespace Elm.Test.Unitest.Options
{
    public class UpdateOptionHandlerTests
    {
        private readonly Mock<IGenericRepository<Option>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateOptionHandler _handler;

        public UpdateOptionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<Option>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateOptionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenOptionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateOptionCommand(99, "Updated content", true);

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync((Option)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Option not found", result.Message);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenOptionFound_UpdatesSuccessfully()
        {
            // Arrange
            var command = new UpdateOptionCommand(1, "Updated content", false);
            var existingOption = new Option
            {
                Id = 1,
                Content = "Original content",
                IsCorrect = true,
                QuestionId = 1
            };
            var updatedOption = new Option
            {
                Id = 1,
                Content = "Updated content",
                IsCorrect = false,
                QuestionId = 1
            };
            var optionDto = new OptionsDto
            {
                Id = 1,
                Content = "Updated content",
                IsCorrect = false
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingOption))
                .ReturnsAsync(updatedOption);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(updatedOption))
                .Returns(optionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Updated content", result.Data.Content);
            Assert.False(result.Data.IsCorrect);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesOptionContent()
        {
            // Arrange
            var command = new UpdateOptionCommand(1, "New content", true);
            var existingOption = new Option
            {
                Id = 1,
                Content = "Old content",
                IsCorrect = false
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Option>()))
                .ReturnsAsync((Option o) => o);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("New content", existingOption.Content);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesIsCorrect()
        {
            // Arrange
            var command = new UpdateOptionCommand(1, "Content", true);
            var existingOption = new Option
            {
                Id = 1,
                Content = "Content",
                IsCorrect = false
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Option>()))
                .ReturnsAsync((Option o) => o);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(existingOption.IsCorrect);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsUpdateAsync()
        {
            // Arrange
            var command = new UpdateOptionCommand(1, "Updated", false);
            var existingOption = new Option { Id = 1, Content = "Original" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingOption))
                .ReturnsAsync(existingOption);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(existingOption), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToOptionsDto()
        {
            // Arrange
            var command = new UpdateOptionCommand(1, "Updated", true);
            var existingOption = new Option { Id = 1, Content = "Original" };
            var updatedOption = new Option { Id = 1, Content = "Updated", IsCorrect = true };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingOption))
                .ReturnsAsync(updatedOption);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(updatedOption))
                .Returns(new OptionsDto { Id = 1, Content = "Updated", IsCorrect = true });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<OptionsDto>(updatedOption), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenOptionNotFound_DoesNotCallUpdateAsync()
        {
            // Arrange
            var command = new UpdateOptionCommand(99, "Content", true);

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.optionId))
                .ReturnsAsync((Option)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Option>()), Times.Never);
        }

        [Theory]
        [InlineData(1, "Content A", true)]
        [InlineData(2, "Content B", false)]
        [InlineData(100, "Long content here", true)]
        public async Task Handle_WithDifferentParameters_UpdatesCorrectly(int optionId, string content, bool isCorrect)
        {
            // Arrange
            var command = new UpdateOptionCommand(optionId, content, isCorrect);
            var existingOption = new Option { Id = optionId, Content = "Old", IsCorrect = !isCorrect };

            _mockRepository
                .Setup(r => r.GetByIdAsync(optionId))
                .ReturnsAsync(existingOption);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Option>()))
                .ReturnsAsync((Option o) => o);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto { Content = content, IsCorrect = isCorrect });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(content, existingOption.Content);
            Assert.Equal(isCorrect, existingOption.IsCorrect);
        }
    }
}
