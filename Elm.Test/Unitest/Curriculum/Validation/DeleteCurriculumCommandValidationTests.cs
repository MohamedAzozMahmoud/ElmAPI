using Elm.Application.Contracts.Features.Curriculum.Commands;
using Elm.Application.Contracts.Validations.Curriculum;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Curriculum.Validation
{
    public class DeleteCurriculumCommandValidationTests
    {
        private readonly DeleteCurriculumCommandValidation _validator;

        public DeleteCurriculumCommandValidationTests()
        {
            _validator = new DeleteCurriculumCommandValidation();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteCurriculumCommand(0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeleteCurriculumCommand(-1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new DeleteCurriculumCommand(1);

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
            var command = new DeleteCurriculumCommand(id);

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
            var command = new DeleteCurriculumCommand(id);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be greater than 0.");
        }

        [Fact]
        public void Validate_WhenIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new DeleteCurriculumCommand(int.MaxValue);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
