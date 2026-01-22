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
    public class AddQuestionHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddQuestionHandler _handler;

        public AddQuestionHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new AddQuestionHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionAddedSuccessfully_ReturnsSuccessResult()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "What is 2 + 2?",
                QuestionType = "MCQ",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "3", IsCorrect = false },
                    new AddOptionsDto { Content = "4", IsCorrect = true },
                    new AddOptionsDto { Content = "5", IsCorrect = false }
                }
            };
            var command = new AddQuestionCommand(1, addQuestionsDto);

            var addedQuestion = new Question
            {
                Id = 1,
                Content = "What is 2 + 2?",
                QuestionType = QuestionType.MCQ,
                QuestionBankId = 1,
                Options = new List<Option>
                {
                    new Option { Id = 1, Content = "3", IsCorrect = false },
                    new Option { Id = 2, Content = "4", IsCorrect = true },
                    new Option { Id = 3, Content = "5", IsCorrect = false }
                }
            };

            var questionDto = new QuestionsDto
            {
                Id = 1,
                Content = "What is 2 + 2?",
                QuestionType = "MCQ",
                Options = new List<OptionsDto>
                {
                    new OptionsDto { Id = 1, Content = "3", IsCorrect = false },
                    new OptionsDto { Id = 2, Content = "4", IsCorrect = true },
                    new OptionsDto { Id = 3, Content = "5", IsCorrect = false }
                }
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .ReturnsAsync(addedQuestion);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(addedQuestion))
                .Returns(questionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("What is 2 + 2?", result.Data.Content);
            Assert.Equal("MCQ", result.Data.QuestionType);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Question>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCalled_CreatesQuestionWithCorrectProperties()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "Is the sky blue?",
                QuestionType = "TrueFalse",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "True", IsCorrect = true },
                    new AddOptionsDto { Content = "False", IsCorrect = false }
                }
            };
            var command = new AddQuestionCommand(5, addQuestionsDto);
            Question capturedQuestion = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .Callback<Question>(q => capturedQuestion = q)
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedQuestion);
            Assert.Equal("Is the sky blue?", capturedQuestion.Content);
            Assert.Equal(QuestionType.TrueFalse, capturedQuestion.QuestionType);
            Assert.Equal(5, capturedQuestion.QuestionBankId);
            Assert.Equal(2, capturedQuestion.Options.Count);
        }

        [Fact]
        public async Task Handle_WithMultipleOptions_CreatesAllOptions()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "Which is a primary color?",
                QuestionType = "MCQ",
                Options = new List<AddOptionsDto>
                {
                    new AddOptionsDto { Content = "Red", IsCorrect = true },
                    new AddOptionsDto { Content = "Green", IsCorrect = false },
                    new AddOptionsDto { Content = "Purple", IsCorrect = false },
                    new AddOptionsDto { Content = "Orange", IsCorrect = false }
                }
            };
            var command = new AddQuestionCommand(1, addQuestionsDto);
            Question capturedQuestion = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .Callback<Question>(q => capturedQuestion = q)
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedQuestion);
            Assert.Equal(4, capturedQuestion.Options.Count);
            Assert.Contains(capturedQuestion.Options, o => o.Content == "Red" && o.IsCorrect);
            Assert.Contains(capturedQuestion.Options, o => o.Content == "Green" && !o.IsCorrect);
        }

        [Fact]
        public async Task Handle_WithMCQType_SetsCorrectQuestionType()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "Test question",
                QuestionType = "MCQ",
                Options = new List<AddOptionsDto>()
            };
            var command = new AddQuestionCommand(1, addQuestionsDto);
            Question capturedQuestion = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .Callback<Question>(q => capturedQuestion = q)
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(QuestionType.MCQ, capturedQuestion.QuestionType);
        }

        [Fact]
        public async Task Handle_WithTrueFalseType_SetsCorrectQuestionType()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "Test question",
                QuestionType = "TrueFalse",
                Options = new List<AddOptionsDto>()
            };
            var command = new AddQuestionCommand(1, addQuestionsDto);
            Question capturedQuestion = null!;

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .Callback<Question>(q => capturedQuestion = q)
                .ReturnsAsync((Question q) => q);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(It.IsAny<Question>()))
                .Returns(new QuestionsDto());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(QuestionType.TrueFalse, capturedQuestion.QuestionType);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_MapsToQuestionsDto()
        {
            // Arrange
            var addQuestionsDto = new AddQuestionsDto
            {
                Content = "Test",
                QuestionType = "MCQ",
                Options = new List<AddOptionsDto>()
            };
            var command = new AddQuestionCommand(1, addQuestionsDto);
            var addedQuestion = new Question { Id = 10, Content = "Test" };
            var expectedDto = new QuestionsDto { Id = 10, Content = "Test" };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Question>()))
                .ReturnsAsync(addedQuestion);

            _mockMapper
                .Setup(m => m.Map<QuestionsDto>(addedQuestion))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<QuestionsDto>(addedQuestion), Times.Once);
            Assert.Equal(10, result.Data!.Id);
        }
    }
}
