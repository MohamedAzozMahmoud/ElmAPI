using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Validations.Subject;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Subject.Validation
{
    public class UpdateSubjectCommandValidationTests
    {
        private readonly UpdateSubjectCommandValidation _validator;

        public UpdateSubjectCommandValidationTests()
        {
            _validator = new UpdateSubjectCommandValidation();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateSubjectCommand(0, "Test Subject", "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Subject Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateSubjectCommand(-1, "Test Subject", "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Subject Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new UpdateSubjectCommand(1, "Test Subject", "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateSubjectCommand(1, "", "CODE001");

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
            var command = new UpdateSubjectCommand(1, null!, "CODE001");

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
            var command = new UpdateSubjectCommand(1, longName, "CODE001");

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
            var command = new UpdateSubjectCommand(1, exactName, "CODE001");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WhenCodeIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new UpdateSubjectCommand(1, "Test Subject", "");

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
            var command = new UpdateSubjectCommand(1, "Test Subject", null!);

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
            var command = new UpdateSubjectCommand(1, "Test Subject", longCode);

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
            var command = new UpdateSubjectCommand(1, "Test Subject", exactCode);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new UpdateSubjectCommand(1, "Mathematics", "MATH101");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(1, "Physics", "PHY101")]
        [InlineData(10, "Chemistry", "CHEM101")]
        [InlineData(100, "Biology", "BIO101")]
        public void Validate_WithValidInputs_ShouldNotHaveAnyValidationErrors(int id, string name, string code)
        {
            // Arrange
            var command = new UpdateSubjectCommand(id, name, code);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenAllFieldsAreInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var command = new UpdateSubjectCommand(0, "", "");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Code);
        }
    }
}
