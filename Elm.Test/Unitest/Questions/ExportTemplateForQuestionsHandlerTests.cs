using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Questions.Queries;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Features.Questions.Handlers;
using Moq;
using System.Linq.Expressions;
using QuestionsBankEntity = Elm.Domain.Entities.QuestionsBank;

namespace Elm.Test.Unitest.Questions
{
    public class ExportTemplateForQuestionsHandlerTests
    {
        private readonly Mock<IQuestionBankRepository> _mockRepository;
        private readonly Mock<IExcelWriter> _mockExcelWriter;
        private readonly ExportTemplateForQuestionsHandler _handler;

        public ExportTemplateForQuestionsHandlerTests()
        {
            _mockRepository = new Mock<IQuestionBankRepository>();
            _mockExcelWriter = new Mock<IExcelWriter>();
            _handler = new ExportTemplateForQuestionsHandler(_mockRepository.Object, _mockExcelWriter.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionBankNotFound_ReturnsFailureResult()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(99);

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync((QuestionsBankEntity)null!);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Question bank not found.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenQuestionBankFound_ReturnsSuccessWithStream()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var questionBank = new QuestionsBankEntity
            {
                Id = 1,
                Name = "Math Questions"
            };
            var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), questionBank.Name))
                .Returns(memoryStream);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(0, result.Data.Position);
        }

        [Fact]
        public async Task Handle_WhenExcelWriterReturnsNull_ReturnsFailureResult()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var questionBank = new QuestionsBankEntity
            {
                Id = 1,
                Name = "Test Bank"
            };

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), questionBank.Name))
                .Returns((MemoryStream)null!);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to generate Excel template.", result.Message);
        }

        [Fact]
        public async Task Handle_WhenExcelWriterReturnsEmptyStream_ReturnsFailureResult()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var questionBank = new QuestionsBankEntity
            {
                Id = 1,
                Name = "Test Bank"
            };
            var emptyStream = new MemoryStream();

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), questionBank.Name))
                .Returns(emptyStream);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to generate Excel template.", result.Message);
        }

        [Fact]
        public async Task Handle_WithValidBankId_CallsFindAsync()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(5);
            var questionBank = new QuestionsBankEntity { Id = 5, Name = "Bank" };

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), It.IsAny<string>()))
                .Returns(new MemoryStream(new byte[] { 1 }));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_UsesQuestionBankNameAsSheetName()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var bankName = "Science Questions";
            var questionBank = new QuestionsBankEntity
            {
                Id = 1,
                Name = bankName
            };

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), bankName))
                .Returns(new MemoryStream(new byte[] { 1 }));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockExcelWriter.Verify(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), bankName), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_PassesEmptyListToExcelWriter()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var questionBank = new QuestionsBankEntity { Id = 1, Name = "Test" };

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), It.IsAny<string>()))
                .Returns(new MemoryStream(new byte[] { 1 }));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockExcelWriter.Verify(w => w.WriteExcelFile(
                It.Is<IReadOnlyList<TemplateQuestionsDto>>(l => l.Count == 0),
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSuccessful_ResetsStreamPosition()
        {
            // Arrange
            var query = new ExportTemplateForQuestionsQuery(1);
            var questionBank = new QuestionsBankEntity { Id = 1, Name = "Test" };
            var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            memoryStream.Position = 3; // Set position to end

            _mockRepository
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<QuestionsBankEntity, bool>>>()))
                .ReturnsAsync(questionBank);

            _mockExcelWriter
                .Setup(w => w.WriteExcelFile(It.IsAny<IReadOnlyList<TemplateQuestionsDto>>(), It.IsAny<string>()))
                .Returns(memoryStream);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data!.Position);
        }
    }
}
