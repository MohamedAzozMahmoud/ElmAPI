using Elm.Application.Contracts.Features.Questions.Commands;
using Elm.Application.Contracts.Validations.Questions;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.QuestionsBank.Validation
{
    public class DeleteQuestionCommandValidationTests
    {
        private readonly DeleteQuestionCommandValidation _validator;

        public DeleteQuestionCommandValidationTests()
        {
            _validator = new DeleteQuestionCommandValidation();
        }

        [Fact]
        public void Validate_WhenQuestionIdIsZero_ShouldHaveValidationError()
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
        public void Validate_WhenQuestionIdIsNegative_ShouldHaveValidationError()
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
        public void Validate_WhenQuestionIdIsPositive_ShouldNotHaveValidationError()
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
        [InlineData(1000)]
        public void Validate_WithValidPositiveIds_ShouldNotHaveAnyValidationErrors(int questionId)
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
        [InlineData(-1000)]
        public void Validate_WithInvalidIds_ShouldHaveValidationError(int questionId)
        {
            // Arrange
            var command = new DeleteQuestionCommand(questionId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.questionId)
                .WithErrorMessage("Question Id must be greater than zero.");
        }

        [Fact]
        public void Validate_WhenQuestionIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = new DeleteQuestionCommand(int.MaxValue);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
