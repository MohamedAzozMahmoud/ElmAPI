using Elm.Application.Contracts.Features.QuestionsBank.Commands;
using Elm.Application.Contracts.Validations.QuestionsBank;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.QuestionsBank.Validation
{
    public class AddQuestionsBankCommandValidationTests
    {
        private readonly AddQuestionsBankCommandValidation _validator;

        public AddQuestionsBankCommandValidationTests()
        {
            _validator = new AddQuestionsBankCommandValidation();
        }

        #region Name Tests

        [Fact]
        public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("", 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.name)
                .WithErrorMessage("Questions bank name is required.");
        }

        [Fact]
        public void Validate_WhenNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand(null!, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.name)
                .WithErrorMessage("Questions bank name is required.");
        }

        [Fact]
        public void Validate_WhenNameExceeds200Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longName = new string('A', 201);
            var command = new AddQuestionsBankCommand(longName, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.name)
                .WithErrorMessage("Questions bank name must not exceed 200 characters.");
        }

        [Fact]
        public void Validate_WhenNameIs200Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactName = new string('A', 200);
            var command = new AddQuestionsBankCommand(exactName, 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.name);
        }

        [Fact]
        public void Validate_WhenNameIsValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Valid Questions Bank Name", 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.name);
        }

        #endregion

        #region CurriculumId Tests

        [Fact]
        public void Validate_WhenCurriculumIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Test Bank", 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.curriculumId)
                .WithErrorMessage("Curriculum ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Test Bank", -1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.curriculumId)
                .WithErrorMessage("Curriculum ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Test Bank", 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.curriculumId);
        }

        #endregion

        #region Combined Tests

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("Math Questions Bank", 1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("Physics Bank", 1)]
        [InlineData("Chemistry Bank", 2)]
        [InlineData("Biology Bank", 3)]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(string name, int curriculumId)
        {
            // Arrange
            var command = new AddQuestionsBankCommand(name, curriculumId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new AddQuestionsBankCommand("", 0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.name);
            result.ShouldHaveValidationErrorFor(x => x.curriculumId);
        }

        #endregion
    }
}
