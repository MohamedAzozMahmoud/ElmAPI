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
    public class AddOptionHandlerTests
    {
        private readonly Mock<IGenericRepository<Option>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddOptionHandler _handler;

        public AddOptionHandlerTests()
        {
            _mockRepository = new Mock<IGenericRepository<Option>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new AddOptionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenOptionAddedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddOptionCommand("Option A", true, 1);
            var addedOption = new Option
            {
                Id = 1,
                Content = "Option A",
                IsCorrect = true,
                QuestionId = 1
            };
            var optionDto = new OptionsDto
            {
                Id = 1,
                Content = "Option A",
                IsCorrect = true
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync(addedOption);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(addedOption))
                .Returns(optionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Option A", result.Data.Content);
            Assert.True(result.Data.IsCorrect);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            var command = new AddOptionCommand("Option B", false, 1);

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync((Option)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add option", result.Message);
        }

        [Fact]
        public async Task Handle_WhenCalled_CreatesOptionWithCorrectProperties()
        {
            // Arrange
            var command = new AddOptionCommand("Test Content", true, 5);
            Option capturedOption = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .Callback<Option>(o => capturedOption = o)
                .ReturnsAsync((Option o) => o);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedOption);
            Assert.Equal("Test Content", capturedOption.Content);
            Assert.True(capturedOption.IsCorrect);
            Assert.Equal(5, capturedOption.QuestionId);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsAddAsync()
        {
            // Arrange
            var command = new AddOptionCommand("Option", false, 1);

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync(new Option { Id = 1 });

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Option>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToOptionsDto()
        {
            // Arrange
            var command = new AddOptionCommand("Option", true, 1);
            var addedOption = new Option { Id = 10, Content = "Option", IsCorrect = true };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync(addedOption);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(addedOption))
                .Returns(new OptionsDto { Id = 10, Content = "Option", IsCorrect = true });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<OptionsDto>(addedOption), Times.Once);
        }

        [Theory]
        [InlineData("Option A", true, 1)]
        [InlineData("Option B", false, 2)]
        [InlineData("Long option content here", true, 100)]
        public async Task Handle_WithDifferentParameters_CreatesCorrectOption(string content, bool isCorrect, int questionId)
        {
            // Arrange
            var command = new AddOptionCommand(content, isCorrect, questionId);
            Option capturedOption = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .Callback<Option>(o => capturedOption = o)
                .ReturnsAsync((Option o) => o);

            _mockMapper
                .Setup(m => m.Map<OptionsDto>(It.IsAny<Option>()))
                .Returns(new OptionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(content, capturedOption.Content);
            Assert.Equal(isCorrect, capturedOption.IsCorrect);
            Assert.Equal(questionId, capturedOption.QuestionId);
        }

        [Fact]
        public async Task Handle_WhenAddFails_DoesNotCallMapper()
        {
            // Arrange
            var command = new AddOptionCommand("Option", true, 1);

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Option>()))
                .ReturnsAsync((Option)null!);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<OptionsDto>(It.IsAny<Option>()), Times.Never);
        }
    }
}
