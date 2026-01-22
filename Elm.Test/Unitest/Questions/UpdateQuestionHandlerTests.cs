using AutoMapper;
using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Elm.Domain.Entities;
using Elm.Domain.Enums;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class UpdateQuestionHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateQuestionHandler _handler;

        public UpdateQuestionHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateQuestionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateQuestionCommand(99, "Updated content", "MCQ");

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Question)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Question not found", result.Message);
            Assert.Equal(404, result.StatusCode);
            _mockRepository.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Question>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenQuestionFound_UpdatesSuccessfully()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Updated question content", "TrueFalse");
            var existingQuestion = new Question
            {
                Id = 1,
                Content = "Original content",
                QuestionType = QuestionType.MCQ,
                QuestionBankId = 1
            };
            var updatedQuestion = new Question
            {
                Id = 1,
                Content = "Updated question content",
                QuestionType = QuestionType.TrueFalse,
                QuestionBankId = 1
            };
            var questionDto = new QuestionsDto
            {
                Id = 1,
                Content = "Updated question content",
                QuestionType = "TrueFalse"
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingQuestion);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Question>()))
                .ReturnsAsync(updatedQuestion);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(updatedQuestion))
                .Returns(questionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Updated question content", result.Data.Content);
            Assert.Equal("TrueFalse", result.Data.QuestionType);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesQuestionContent()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "New content", "MCQ");
            var existingQuestion = new Question
            {
                Id = 1,
                Content = "Old content",
                QuestionType = QuestionType.MCQ
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingQuestion);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Question>()))
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("New content", existingQuestion.Content);
        }

        [Fact]
        public async Task Handle_WhenCalled_UpdatesQuestionType()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Content", "TrueFalse");
            var existingQuestion = new Question
            {
                Id = 1,
                Content = "Content",
                QuestionType = QuestionType.MCQ
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingQuestion);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Question>()))
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(QuestionType.TrueFalse, existingQuestion.QuestionType);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_CallsUpdateAsync()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Updated", "MCQ");
            var existingQuestion = new Question { Id = 1, Content = "Original" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingQuestion);

            _mockRepository
                .Setup(r => r.UpdateAsync(existingQuestion))
                .ReturnsAsync(existingQuestion);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(existingQuestion), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToDto()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Updated", "MCQ");
            var existingQuestion = new Question { Id = 1, Content = "Original" };
            var updatedQuestion = new Question { Id = 1, Content = "Updated" };
            var expectedDto = new QuestionsDto { Id = 1, Content = "Updated" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(existingQuestion);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Question>()))
                .ReturnsAsync(updatedQuestion);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(updatedQuestion))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<QuestionsDto>(updatedQuestion), Times.Once);
            Assert.Equal(expectedDto, result.Data);
        }
    }
}
