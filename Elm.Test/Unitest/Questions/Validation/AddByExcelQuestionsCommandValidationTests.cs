using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Questions.Validation
{
    public class AddByExcelQuestionsCommandValidationTests
    {
        private readonly AddByExcelQuestionsCommandValidation _validator;

        public AddByExcelQuestionsCommandValidationTests()
        {
            _validator = new AddByExcelQuestionsCommandValidation();
        }

        #region QuestionBankId Validation Tests

        [Fact]
        public void Validate_WhenQuestionBankIdIsZero_ShouldHaveError()
        {
            // Arrange
            var excelFile = CreateValidExcelStream();
            var command = new AddByExcelQuestionsCommand(0, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionBankId)
                .WithErrorMessage("Question Bank Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionBankIdIsNegative_ShouldHaveError()
        {
            // Arrange
            var excelFile = CreateValidExcelStream();
            var command = new AddByExcelQuestionsCommand(-1, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionBankId)
                .WithErrorMessage("Question Bank Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionBankIdIsPositive_ShouldNotHaveError()
        {
            // Arrange
            var excelFile = CreateValidExcelStream();
            var command = new AddByExcelQuestionsCommand(1, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.questionBankId);
        }

        #endregion

        #region ExcelFile Validation Tests

        [Fact]
        public void Validate_WhenExcelFileIsEmpty_ShouldHaveError()
        {
            // Arrange
            var emptyStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(1, emptyStream);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExcelFile)
                .WithErrorMessage("Excel file cannot be empty.");
        }

        [Fact]
        public void Validate_WhenExcelFileExceeds10MB_ShouldHaveError()
        {
            // Arrange
            var largeFile = new MemoryStream(new byte[10485761]); // 10 MB + 1 byte
            var command = new AddByExcelQuestionsCommand(1, largeFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExcelFile)
                .WithErrorMessage("Excel file must be less than 10 MB.");
        }

        [Fact]
        public void Validate_WhenExcelFileIsExactly10MB_ShouldHaveError()
        {
            // Arrange
            var exactFile = new MemoryStream(new byte[10485760]); // Exactly 10 MB
            var command = new AddByExcelQuestionsCommand(1, exactFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExcelFile)
                .WithErrorMessage("Excel file must be less than 10 MB.");
        }

        [Fact]
        public void Validate_WhenExcelFileIsLessThan10MB_ShouldNotHaveError()
        {
            // Arrange
            var validFile = new MemoryStream(new byte[10485759]); // 10 MB - 1 byte
            var command = new AddByExcelQuestionsCommand(1, validFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ExcelFile);
        }

        [Fact]
        public void Validate_WhenExcelFileHasContent_ShouldNotHaveError()
        {
            // Arrange
            var excelFile = CreateValidExcelStream();
            var command = new AddByExcelQuestionsCommand(1, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ExcelFile);
        }

        #endregion

        #region Full Command Validation Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyErrors()
        {
            // Arrange
            var excelFile = CreateValidExcelStream();
            var command = new AddByExcelQuestionsCommand(1, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleErrors()
        {
            // Arrange
            var emptyStream = new MemoryStream();
            var command = new AddByExcelQuestionsCommand(0, emptyStream);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionBankId);
            result.ShouldHaveValidationErrorFor(x => x.ExcelFile);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(1048576)] // 1 MB
        public void Validate_WhenExcelFileHasValidSize_ShouldNotHaveError(int fileSize)
        {
            // Arrange
            var excelFile = new MemoryStream(new byte[fileSize]);
            var command = new AddByExcelQuestionsCommand(1, excelFile);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ExcelFile);
        }

        #endregion

        #region Helper Methods

        private static MemoryStream CreateValidExcelStream()
        {
            return new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        }

        #endregion
    }
}
