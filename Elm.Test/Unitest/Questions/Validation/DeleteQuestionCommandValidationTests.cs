using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Questions.Validation
{
    public class DeleteQuestionCommandValidationTests
    {
        private readonly DeleteQuestionCommandValidation _validator;

        public DeleteQuestionCommandValidationTests()
        {
            _validator = new DeleteQuestionCommandValidation();
        }

        [Fact]
        public void Validate_WhenQuestionIdIsZero_ShouldHaveError()
        {
            // Arrange
            var command = new DeleteQuestionCommand(0);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionId)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionIdIsNegative_ShouldHaveError()
        {
            // Arrange
            var command = new DeleteQuestionCommand(-1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionId)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionIdIsPositive_ShouldNotHaveError()
        {
            // Arrange
            var command = new DeleteQuestionCommand(1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.questionId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Validate_WhenQuestionIdIsValid_ShouldNotHaveError(int questionId)
        {
            // Arrange
            var command = new DeleteQuestionCommand(questionId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Validate_WhenQuestionIdIsInvalid_ShouldHaveError(int questionId)
        {
            // Arrange
            var command = new DeleteQuestionCommand(questionId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionId);
        }
    }
}
