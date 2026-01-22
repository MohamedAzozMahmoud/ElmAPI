using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;

namespace Elm.Test.Unitest.Questions
{
    public class AddByExcelQuestionsHandlerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly Mock<IExcelReader> _mockExcelReader;
        private readonly AddByExcelQuestionsHandler _handler;

        public AddByExcelQuestionsHandlerTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _mockExcelReader = new Mock<IExcelReader>();
            _handler = new AddByExcelQuestionsHandler(_mockRepository.Object, _mockExcelReader.Object);
        }

        [Fact]
        public async Task Handle_WhenValidExcelData_ReturnsSuccessResult()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto
                {
                    Content = "What is 2 + 2?",
                    QuestionType = "MCQ",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "5",
                    OptionD = "6",
                    CorrectOption = "B"
                }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            _mockRepository
                .Setup(r => r.AddRingQuestionsFromExcel(command.questionBankId, It.IsAny<List<TemplateQuestionsDto>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_WhenQuestionContentIsEmpty_ReturnsFailureResult()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto
                {
                    Content = "", // Empty content
                    QuestionType = "MCQ"
                }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid question data found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenQuestionTypeIsEmpty_ReturnsFailureResult()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto
                {
                    Content = "Valid content",
                    QuestionType = "" // Empty question type
                }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid question data found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenQuestionContentIsWhitespace_ReturnsFailureResult()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto
                {
                    Content = "   ", // Whitespace only
                    QuestionType = "MCQ"
                }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid question data found.", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidData_CallsReadExcelFile()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto { Content = "Q1", QuestionType = "MCQ" }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            _mockRepository
                .Setup(r => r.AddRingQuestionsFromExcel(command.questionBankId, It.IsAny<List<TemplateQuestionsDto>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockExcelReader.Verify(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidData_CallsAddRingQuestionsFromExcel()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var bankId = 5;
            var command = new AddByExcelQuestionsCommand(bankId, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto { Content = "Q1", QuestionType = "MCQ" }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            _mockRepository
                .Setup(r => r.AddRingQuestionsFromExcel(bankId, It.IsAny<List<TemplateQuestionsDto>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestionsFromExcel(bankId, It.IsAny<List<TemplateQuestionsDto>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenMixedValidAndInvalidData_ReturnsFailure()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto { Content = "Valid question", QuestionType = "MCQ" },
                new TemplateQuestionsDto { Content = "", QuestionType = "MCQ" } // Invalid
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid question data found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenAllQuestionsValid_PassesAllToRepository()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto { Content = "Q1", QuestionType = "MCQ" },
                new TemplateQuestionsDto { Content = "Q2", QuestionType = "TrueFalse" },
                new TemplateQuestionsDto { Content = "Q3", QuestionType = "MCQ" }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            _mockRepository
                .Setup(r => r.AddRingQuestionsFromExcel(command.questionBankId, It.IsAny<List<TemplateQuestionsDto>>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestionsFromExcel(1, It.Is<List<TemplateQuestionsDto>>(l => l.Count == 3)), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenInvalidData_DoesNotCallRepository()
        {
            // Arrange
            var excelStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, excelStream);
            var templateQuestions = new List<TemplateQuestionsDto>
            {
                new TemplateQuestionsDto { Content = "", QuestionType = "" }
            };

            _mockExcelReader
                .Setup(r => r.ReadExcelFile<TemplateQuestionsDto>(excelStream))
                .Returns(templateQuestions);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.AddRingQuestionsFromExcel(It.IsAny<int>(), It.IsAny<List<TemplateQuestionsDto>>()), Times.Never);
        }
    }
}
