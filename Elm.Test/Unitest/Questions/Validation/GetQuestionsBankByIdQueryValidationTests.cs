using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Validations.QuestionsBank;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.Questions.Validation
{
    public class GetQuestionsBankByIdQueryValidationTests
    {
        private readonly GetQuestionsBankByIdQueryValidation _validator;

        public GetQuestionsBankByIdQueryValidationTests()
        {
            _validator = new GetQuestionsBankByIdQueryValidation();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveError()
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id)
                .WithErrorMessage("Questions bank ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldHaveError()
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(-1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id)
                .WithErrorMessage("Questions bank ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldNotHaveError()
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public void Validate_WhenIdIsValid_ShouldNotHaveError(int id)
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(id);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Validate_WhenIdIsInvalid_ShouldHaveError(int id)
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(id);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id);
        }

        [Fact]
        public void Validate_WhenIdIsOne_ShouldNotHaveError()
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenIdIsMaxValue_ShouldNotHaveError()
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(int.MaxValue);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
