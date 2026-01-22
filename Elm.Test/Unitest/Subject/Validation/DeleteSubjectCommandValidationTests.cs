using Elm.Application.Contracts.Features.Subject.Commands;
using Elm.Application.Contracts.Validations.Subject;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Subject.Validation
{
    public class DeleteSubjectCommandValidationTests
    {
        private readonly DeleteSubjectCommandValidation _validator;

        public DeleteSubjectCommandValidationTests()
        {
            _validator = new DeleteSubjectCommandValidation();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteSubjectCommand(0);

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
            var command = new DeleteSubjectCommand(-1);

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
            var command = new DeleteSubjectCommand(1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Validate_WithValidPositiveIds_ShouldNotHaveAnyValidationErrors(int id)
        {
            // Arrange
            var command = new DeleteSubjectCommand(id);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(-1000)]
        public void Validate_WithInvalidIds_ShouldHaveValidationError(int id)
        {
            // Arrange
            var command = new DeleteSubjectCommand(id);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Subject Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new DeleteSubjectCommand(int.MaxValue);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
