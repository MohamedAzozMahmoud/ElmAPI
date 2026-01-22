using Elm.Application.Contracts.Features.QuestionsBank.Queries;
using Elm.Application.Contracts.Validations.QuestionsBank;
using FluentValidation.TestHelper;

namespace Elm.Test.Unitest.QuestionsBank.Validation
{
    public class GetAllQuestionsBankQueryValidationTests
    {
        private readonly GetAllQuestionsBankQueryValidation _validator;

        public GetAllQuestionsBankQueryValidationTests()
        {
            _validator = new GetAllQuestionsBankQueryValidation();
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.curriculumId)
                .WithErrorMessage("Curriculum ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(-1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.curriculumId)
                .WithErrorMessage("Curriculum ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsPositive_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.curriculumId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Validate_WithValidPositiveCurriculumIds_ShouldNotHaveAnyValidationErrors(int curriculumId)
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(curriculumId);

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
        public void Validate_WithInvalidCurriculumIds_ShouldHaveValidationError(int curriculumId)
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(curriculumId);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.curriculumId)
                .WithErrorMessage("Curriculum ID must be a positive integer.");
        }

        [Fact]
        public void Validate_WhenCurriculumIdIsMaxInt_ShouldNotHaveValidationError()
        {
            // Arrange
            var query = new GetAllQuestionsBankQuery(int.MaxValue);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
