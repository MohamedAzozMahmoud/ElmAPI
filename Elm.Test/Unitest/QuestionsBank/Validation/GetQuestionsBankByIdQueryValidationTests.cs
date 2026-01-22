using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Validations.QuestionsBank;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.QuestionsBank.Validation
{
    public class GetQuestionsBankByIdQueryValidationTests
    {
        private readonly GetQuestionsBankByIdQueryValidation _validator;

        public GetQuestionsBankByIdQueryValidationTests()
        {
            _validator = new GetQuestionsBankByIdQueryValidation();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldHaveValidationError()
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
        public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
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
        public void Validate_WhenIdIsPositive_ShouldNotHaveValidationError()
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
        public void Validate_WithValidPositiveIds_ShouldNotHaveAnyValidationErrors(int id)
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
        [InlineData(-100)]
        [InlineData(-1000)]
        public void Validate_WithInvalidIds_ShouldHaveValidationError(int id)
        {
            // Arrange
            var query = new GetQuestionsBankByIdQuery(id);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id)
                .WithErrorMessage("Questions bank ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenIdIsMaxInt_ShouldNotHaveValidationError()
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
