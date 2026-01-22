using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Validations.Subject;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Subject.Validation
{
    public class AddSubjectCommandValidationTests
    {
        private readonly AddSubjectCommandValidation _validator;

        public AddSubjectCommandValidationTests()
        {
            _validator = new AddSubjectCommandValidation();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddSubjectCommand("", "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Subject name is required.");
        }

        [Fact]
        public void Validate_WhenNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddSubjectCommand(null!, "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Subject name is required.");
        }

        [Fact]
        public void Validate_WhenNameExceeds100Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longName = new string('A', 101);
            var command = new AddSubjectCommand(longName, "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Subject name must not exceed 100 characters.");
        }

        [Fact]
        public void Validate_WhenNameIs100Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactName = new string('A', 100);
            var command = new AddSubjectCommand(exactName, "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenCodeIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddSubjectCommand("Test Subject", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Subject code is required.");
        }

        [Fact]
        public void Validate_WhenCodeIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AddSubjectCommand("Test Subject", null!);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Subject code is required.");
        }

        [Fact]
        public void Validate_WhenCodeExceeds20Characters_ShouldHaveValidationError()
        {
            // Arrange
            var longCode = new string('A', 21);
            var command = new AddSubjectCommand("Test Subject", longCode);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Subject code must not exceed 20 characters.");
        }

        [Fact]
        public void Validate_WhenCodeIs20Characters_ShouldNotHaveValidationError()
        {
            // Arrange
            var exactCode = new string('A', 20);
            var command = new AddSubjectCommand("Test Subject", exactCode);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new AddSubjectCommand("Mathematics", "MATH101");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("Physics", "PHY101")]
        [InlineData("Chemistry", "CHEM101")]
        [InlineData("Biology", "BIO101")]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(string name, string code)
        {
            // Arrange
            var command = new AddSubjectCommand(name, code);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
