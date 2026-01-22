using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.QuestionsBank.Validation
{
    public class UpdateQuestionCommandValidationTests
    {
        private readonly UpdateQuestionCommandValidation _validator;

        public UpdateQuestionCommandValidationTests()
        {
            _validator = new UpdateQuestionCommandValidation();
        }

        #region Id Tests

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(0, "Content", "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(-1, "Content", "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Content", "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        #endregion

        #region Content Tests

        [Fact]
        public void Validate_WhenContentIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "", "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Question content must not be empty.");
        }

        [Fact]
        public void Validate_WhenContentIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, null!, "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Question content must not be empty.");
        }

        [Fact]
        public void Validate_WhenContentExceeds1000Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longContent = new string('A', 1001);
            var command = new UpdateQuestionCommand(1, longContent, "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Question content must not exceed 1000 characters.");
        }

        [Fact]
        public void Validate_WhenContentIs1000Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactContent = new string('A', 1000);
            var command = new UpdateQuestionCommand(1, exactContent, "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validate_WhenContentIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "What is 2+2?", "Type");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
        }

        #endregion

        #region QuestionType Tests

        [Fact]
        public void Validate_WhenQuestionTypeIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Content", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType)
                .WithErrorMessage("Question type must not be empty.");
        }

        [Fact]
        public void Validate_WhenQuestionTypeIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Content", null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType)
                .WithErrorMessage("Question type must not be empty.");
        }

        [Fact]
        public void Validate_WhenQuestionTypeExceeds100Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longType = new string('A', 101);
            var command = new UpdateQuestionCommand(1, "Content", longType);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.QuestionType)
                .WithErrorMessage("Question type must not exceed 100 characters.");
        }

        [Fact]
        public void Validate_WhenQuestionTypeIs100Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactType = new string('A', 100);
            var command = new UpdateQuestionCommand(1, "Content", exactType);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        [Fact]
        public void Validate_WhenQuestionTypeIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Content", "MultipleChoice");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "What is the capital of France?", "MultipleChoice");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, "Question 1", "TrueFalse")]
        [InlineData(10, "Question 2", "MultipleChoice")]
        [InlineData(100, "Question 3", "Essay")]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(int id, string content, string questionType)
        {
            // Arrange
            var command = new UpdateQuestionCommand(id, content, questionType);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
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

        [Fact]
        public void Validate_WhenOnlyIdIsInvalid_ShouldHaveOnlyIdValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(0, "Valid Content", "ValidType");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        [Fact]
        public void Validate_WhenOnlyContentIsInvalid_ShouldHaveOnlyContentValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "", "ValidType");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Content);
            result.ShouldNotHaveValidationErrorFor(x => x.QuestionType);
        }

        [Fact]
        public void Validate_WhenOnlyQuestionTypeIsInvalid_ShouldHaveOnlyQuestionTypeValidationError()
        {
            // Arrange
            var command = new UpdateQuestionCommand(1, "Valid Content", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.Content);
            result.ShouldHaveValidationErrorFor(x => x.QuestionType);
        }

        #endregion
    }
}
