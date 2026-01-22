using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Questions.Validation
{
    public class UpdateQuestionCommandValidationTests
    {
        private readonly UpdateQuestionCommandValidation _validator;

        public UpdateQuestionCommandValidationTests()
        {
            _validator = new UpdateQuestionCommandValidation();
        }

        #region Id Validation Tests

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(0, "Valid content", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(-1, "Valid content", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Valid content", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        #endregion

        #region Content Validation Tests

        [Fact]
        public void Validate_WhenContentIsEmpty_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Question content must not be empty.");
        }

        [Fact]
        public void Validate_WhenContentIsNull_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, null!, "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validate_WhenContentIsWhitespace_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "   ", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validate_WhenContentExceeds1000Characters_ShouldHaveError()
        {
            // Arrange
            var longContent = new string('a', 1001);
            var command = new UpdateQuestionCommand(1, longContent, "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Question content must not exceed 1000 characters.");
        }

        [Fact]
        public void Validate_WhenContentIsExactly1000Characters_ShouldNotHaveError()
        {
            // Arrange
            var content = new string('a', 1000);
            var command = new UpdateQuestionCommand(1, content, "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validate_WhenContentIsValid_ShouldNotHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "What is 2 + 2?", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
        }

        #endregion

        #region QuestionType Validation Tests

        [Fact]
        public void Validate_WhenQuestionTypeIsEmpty_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Valid content", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType)
                .WithErrorMessage("Question type must not be empty.");
        }

        [Fact]
        public void Validate_WhenQuestionTypeIsNull_ShouldHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Valid content", null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType);
        }

        [Fact]
        public void Validate_WhenQuestionTypeExceeds100Characters_ShouldHaveError()
        {
            // Arrange
            var longType = new string('a', 101);
            var command = new UpdateQuestionCommand(1, "Valid content", longType);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType)
                .WithErrorMessage("Question type must not exceed 100 characters.");
        }

        [Fact]
        public void Validate_WhenQuestionTypeIsExactly100Characters_ShouldNotHaveError()
        {
            // Arrange
            var questionType = new string('a', 100);
            var command = new UpdateQuestionCommand(1, "Valid content", questionType);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        [Fact]
        public void Validate_WhenQuestionTypeIsValid_ShouldNotHaveError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Valid content", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        #endregion

        #region Full Command Validation Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyErrors()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "What is 2 + 2?", "MCQ");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleErrors()
        {
            // Arrange
            var command = new UpdateQuestionCommand(0, "", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Content);
            result.ShouldHaveValidationErrorFor(x => x.QuestionType);
        }

        #endregion
    }
}
